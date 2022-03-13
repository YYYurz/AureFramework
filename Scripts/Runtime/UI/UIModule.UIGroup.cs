//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System.Collections.Generic;
using AureFramework.Event;
using AureFramework.ObjectPool;
using AureFramework.ReferencePool;
using UnityEngine;

namespace AureFramework.UI
{
	public sealed partial class UIModule : AureFrameworkModule, IUIModule
	{
		/// <summary>
		/// UI组
		/// </summary>
		private sealed partial class UIGroup : IUIGroup
		{
			private readonly IObjectPool<UIObject> uiObjectPool;
			private readonly IEventModule eventModule;
			private readonly IReferencePoolModule referencePoolModule;
			private readonly LinkedList<UIFormInfo> uiFormInfoLinked = new LinkedList<UIFormInfo>();
			private readonly Queue<UITask> waitingUITaskQue = new Queue<UITask>();
			private readonly Transform groupRoot;
			private readonly UIGroupAdapter uiGroupAdapter;
			private readonly string groupName;
			private int groupDepth;
			private float waitTime;
			private const float TaskExpireTime = 100f;

			public UIGroup(IObjectPool<UIObject> uiObjectPool, string groupName, int groupDepth, Transform groupRoot, UIGroupAdapter uiGroupAdapter)
			{
				this.uiObjectPool = uiObjectPool;
				this.groupName = groupName;
				this.groupDepth = groupDepth;
				this.groupRoot = groupRoot;
				this.uiGroupAdapter = uiGroupAdapter;
				eventModule = Aure.GetModule<IEventModule>();
				referencePoolModule = Aure.GetModule<IReferencePoolModule>();
			}

			/// <summary>
			/// 获取UI组名称
			/// </summary>
			public string GroupName
			{
				get
				{
					return groupName;
				}
			}

			/// <summary>
			/// 获取或设置UI组深度
			/// </summary>
			public int GroupDepth
			{
				get
				{
					return groupDepth;
				}
				set
				{
					if (groupDepth == value)
					{
						return;
					}

					groupDepth = value;
					uiGroupAdapter.SetDepth(groupDepth);
				}
			}

			/// <summary>
			/// 轮询打开的UI
			/// 处理未处理的UI操作
			/// </summary>
			/// <param name="realElapseTime"> 真实流逝时间 </param>
			public void InternalUpdate(float realElapseTime)
			{
				waitTime += realElapseTime;
				waitTime = InternalTryProcessTask() ? 0f : waitTime;
				if (waitingUITaskQue.Count == 0)
				{
					waitTime = 0f;
				}
				else if (waitingUITaskQue.Count > 0 && waitTime > TaskExpireTime)
				{
					var expireTask = waitingUITaskQue.Dequeue();
					eventModule.Fire(this, OpenUIFailedEventArgs.Create(expireTask.UIName, $"Open UI task has expired, UI name :{expireTask.UIName}"));
					referencePoolModule.Release(expireTask);
					waitTime = 0f;
				}

				foreach (var uiFormInfo in uiFormInfoLinked)
				{
					uiFormInfo.UIFormBase.OnUpdate(realElapseTime);
				}
			}

			/// <summary>
			/// 是否存在已打开UI
			/// </summary>
			/// <param name="uiName"> UI名称 </param>
			/// <returns></returns>
			public bool IsHasUI(string uiName)
			{
				var uiNode = GetUINode(uiName);
				return uiNode != null;
			}

			/// <summary>
			/// 获取已打开的UIForm
			/// </summary>
			/// <param name="uiName"> UI名称 </param>
			/// <returns></returns>
			public UIFormBase GetUIForm(string uiName)
			{
				var uiNode = GetUINode(uiName);
				return uiNode?.Value.UIFormBase;
			}

			/// <summary>
			/// 获取所有已打开UIForm
			/// </summary>
			/// <returns></returns>
			public UIFormBase[] GetAllUIForm()
			{
				var result = new UIFormBase[uiFormInfoLinked.Count];
				var curNode = uiFormInfoLinked.First;
				var index = 0;
				while (curNode != null)
				{
					result[index] = curNode.Value.UIFormBase;
					curNode = curNode.Next;
					index++;
				}

				return result;
			}

			/// <summary>
			/// 入队打开UI操作
			/// </summary>
			/// <param name="uiName"> UI名称 </param>
			/// <param name="uiAssetName"> UI资源名称 </param>
			/// <param name="userData"> 用户数据 </param>
			public void OpenUI(string uiName, string uiAssetName, object userData)
			{
				InternalCreateUITask(uiName, uiAssetName, UITaskType.OpenUI, userData);
			}

			/// <summary>
			/// 入队关闭UI操作
			/// </summary>
			/// <param name="uiName"> UI名称 </param>
			public void CloseUI(string uiName)
			{
				InternalCreateUITask(uiName, null,  UITaskType.CloseUI, null);
			}

			/// <summary>
			/// 清除队列中所有未处理操作，关闭所有已打开UI
			/// </summary>
			public void CloseAllUI()
			{
				CloseAllExcept();
			}

			/// <summary>
			/// 除了传入UI，关闭所有已打开UI
			/// </summary>
			/// <param name="uiName"> UI名称 </param>
			public void CloseAllExcept(string uiName = null)
			{
				ClearAllUITask();
				var curNode = uiFormInfoLinked.Last;
				while (curNode != null)
				{
					var tempNode = curNode;
					curNode = curNode.Previous;

					if (!tempNode.Value.UIName.Equals(uiName))
					{
						InternalCreateUITask(tempNode.Value.UIName, null, UITaskType.CloseUI, null);
					}
				}
			}

			public void ClearAllUITask()
			{
				foreach (var uiTask in waitingUITaskQue)
				{
					referencePoolModule.Release(uiTask);
				}

				waitingUITaskQue.Clear();
			}

			public void DiscardUITask(string uiName)
			{
				foreach (var uiTask in waitingUITaskQue)
				{
					if (uiTask.UIName.Equals(uiName))
					{
						uiTask.UITaskType = UITaskType.Complete;
					}
				}
			}

			private void InternalCreateUITask(string uiName, string uiAssetName, UITaskType uiTaskType, object userData)
			{
				waitingUITaskQue.Enqueue(UITask.Create(uiName, uiAssetName, uiTaskType, userData));
				InternalTryProcessTask();
			}

			private bool InternalTryProcessTask()
			{
				if (waitingUITaskQue.Count == 0)
				{
					return true;
				}

				var processTaskNum = 0;
				while (waitingUITaskQue.Count > 0)
				{
					var uiTask = waitingUITaskQue.Peek();
					switch (uiTask.UITaskType)
					{
						case UITaskType.OpenUI:
							InternalTryOpenUI(uiTask);
							break;
						case UITaskType.CloseUI:
							InternalTryCloseUI(uiTask);
							break;
						case UITaskType.None:
							break;
						case UITaskType.Complete:
							break;
					}

					if (uiTask.UITaskType == UITaskType.Complete || uiTask.UITaskType == UITaskType.None)
					{
						referencePoolModule.Release(waitingUITaskQue.Dequeue());
						processTaskNum++;
					}
					else
					{
						break;
					}

					Refresh();
				}

				return processTaskNum > 0;
			}

			private void InternalTryOpenUI(UITask uiTask)
			{
				var uiNode = GetUINode(uiTask.UIName);
				if (uiNode != null)
				{
					uiNode.Value.UIFormBase.OnOpen(uiTask.UserData);
					uiFormInfoLinked.Remove(uiNode);
					uiFormInfoLinked.AddLast(uiNode);
					uiTask.UITaskType = UITaskType.Complete;
					eventModule.Fire(this, OpenUISuccessEventArgs.Create(uiNode.Value.UIFormBase));
					return;
				}

				if (InternalTrySpawnUIObject(uiTask.UIAssetName, out var uiObject))
				{
					uiObject.UIGameObject.transform.SetParent(groupRoot);
					uiObject.UIGameObject.SetActive(true);

					var uiForm = uiObject.UIGameObject.GetComponent<UIFormBase>();
					if (!uiForm.IsAlreadyInit)
					{
						uiForm.OnInit(uiTask.UserData);
					}

					uiForm.OnOpen(uiTask.UserData);

					uiFormInfoLinked.AddLast(UIFormInfo.Create(uiForm, uiObject, uiTask.UIName, uiTask.UIAssetName));
					uiTask.UITaskType = UITaskType.Complete;
					eventModule.Fire(this, OpenUISuccessEventArgs.Create(uiForm));
				}
			}

			private void InternalTryCloseUI(UITask uiTask)
			{
				var uiNode = GetUINode(uiTask.UIName);
				if (uiNode != null)
				{
					uiNode.Value.UIFormBase.OnClose();
					uiNode.Value.UIObject.UIGameObject.SetActive(false);
					uiObjectPool.Recycle(uiNode.Value.UIObject);

					uiFormInfoLinked.Remove(uiNode);
					referencePoolModule.Release(uiNode.Value);
				}

				uiTask.UITaskType = UITaskType.Complete;
			}

			private bool InternalTrySpawnUIObject(string uiAssetName, out UIObject uiObject)
			{
				uiObject = uiObjectPool.Spawn(uiAssetName);

				var uiForm = uiObject?.UIGameObject.GetComponent<UIFormBase>();
				if (uiForm != null)
				{
					return true;
				}

				return false;
			}

			private void Refresh()
			{
				var curNode = uiFormInfoLinked.Last;
				var curDepth = uiFormInfoLinked.Count * 100;
				var isTop = true;
				while (curNode != null)
				{
					if (isTop)
					{
						if (curNode.Value.IsPause)
						{
							curNode.Value.UIFormBase.OnResume();
						}

						curNode.Value.IsPause = false;
						isTop = false;
					}
					else
					{
						if (!curNode.Value.IsPause)
						{
							curNode.Value.UIFormBase.OnPause();
							curNode.Value.IsPause = true;
						}
					}

					if (curNode.Value.Depth != curDepth)
					{
						curNode.Value.UIFormBase.OnDepthChange(curDepth);
					}

					curNode.Value.Depth = curDepth;
					curDepth -= 100;

					curNode = curNode.Previous;
				}
			}

			private LinkedListNode<UIFormInfo> GetUINode(string uiName)
			{
				var curNode = uiFormInfoLinked.First;
				while (curNode != null)
				{
					if (curNode.Value.UIName.Equals(uiName))
					{
						break;
					}

					curNode = curNode.Next;
				}

				return curNode;
			}
		}
	}
}