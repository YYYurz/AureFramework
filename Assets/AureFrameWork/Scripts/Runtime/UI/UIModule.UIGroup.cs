//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using AureFramework.ObjectPool;
using AureFramework.ReferencePool;
using UnityEngine;

namespace AureFramework.UI {
	public sealed partial class UIModule : AureFrameworkModule, IUIModule {
		/// <summary>
		/// UI组
		/// </summary>
		private sealed partial class UIGroup : IUIGroup {
			private readonly IObjectPool<GameObject> uiObjectPool;
			private readonly IReferencePoolModule referencePoolModule;
			private readonly Dictionary<string, IObject<GameObject>> usingUIObject = new Dictionary<string, IObject<GameObject>>();
			private readonly LinkedList<UIFormInfo> uiFormInfoLinked = new LinkedList<UIFormInfo>();
			private readonly Queue<UITask> waitingUITaskQue = new Queue<UITask>();
			private readonly Transform groupRoot;
			private readonly string groupName;
			private readonly int groupDepth;
			private int curUIDepth;
			private float waitTime;
			private const float taskExpireTime = 5f;

			public UIGroup(IObjectPool<GameObject> uiObjectPool, string groupName, int groupDepth, Transform groupRoot) {
				this.uiObjectPool = uiObjectPool;
				this.groupName = groupName;
				this.groupDepth = groupDepth;
				this.groupRoot = groupRoot;
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
			/// 获取UI组深度
			/// </summary>
			public int GroupDepth
			{
				get
				{
					return groupDepth;
				}
			}

			/// <summary>
			/// 轮询打开的UI
			/// 处理未处理的UI操作
			/// </summary>
			/// <param name="realElapseTime"> 真实流逝时间 </param>
			public void Update(float realElapseTime) {
				waitTime += realElapseTime;
				waitTime = InternalTryProcessTask() ? 0f : waitTime;
				if (waitingUITaskQue.Count == 0) {
					waitTime = 0f;
				} else if (waitingUITaskQue.Count > 0 && waitTime > taskExpireTime) {
					referencePoolModule.Release(waitingUITaskQue.Dequeue());
					waitTime = 0f;
				}

				foreach (var uiFormInfo in uiFormInfoLinked) {
					uiFormInfo.UIFormBase.OnUpdate(realElapseTime);
				}
			}

			/// <summary>
			/// 是否存在已打开UI
			/// </summary>
			/// <param name="uiName"> UI名称 </param>
			/// <returns></returns>
			public bool IsHasUI(string uiName) {
				var uiNode = GetUINode(uiName);
				return uiNode == null;
			}

			/// <summary>
			/// 获取已打开的UIForm
			/// </summary>
			/// <param name="uiName"> UI名称 </param>
			/// <returns></returns>
			public UIFormBase GetUIForm(string uiName) {
				var uiNode = GetUINode(uiName);
				return uiNode?.Value.UIFormBase;
			}

			/// <summary>
			/// 获取所有已打开UIForm
			/// </summary>
			/// <returns></returns>
			public UIFormBase[] GetAllUIForm() {
				var result = new UIFormBase[uiFormInfoLinked.Count];
				var curNode = uiFormInfoLinked.First;
				var index = 0;
				while (curNode != null) {
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
			/// <param name="userData"> 用户数据 </param>
			public void OpenUI(string uiName, object userData) {
				InternalCreateUITask(uiName, UITaskType.OpenUI, userData);
			}

			/// <summary>
			/// 入队关闭UI操作
			/// </summary>
			/// <param name="uiName"> UI名称 </param>
			public void CloseUI(string uiName) {
				InternalCreateUITask(uiName, UITaskType.CloseUI, null);
			}

			/// <summary>
			/// 清除队列中所有未处理操作，关闭所有已打开UI
			/// </summary>
			public void CloseAllUI() {
				CloseAllExcept();
			}

			/// <summary>
			/// 除了传入UI，关闭所有已打开UI
			/// </summary>
			/// <param name="uiName"> UI名称 </param>
			public void CloseAllExcept(string uiName = null) {
				ClearAllUITask();
				var curNode = uiFormInfoLinked.Last;
				while (curNode != null) {
					if (curNode.Value.UIName.Equals(uiName)) {
						InternalCreateUITask(curNode.Value.UIName, UITaskType.CloseUI, null);
					}
					curNode = curNode.Previous;
				}
			}

			public void ClearAllUITask() {
				foreach (var uiTask in waitingUITaskQue) {
					referencePoolModule.Release(uiTask);
				}
				waitingUITaskQue.Clear();
			}

			public void DiscardUITask(string uiName) {
				foreach (var uiTask in waitingUITaskQue) {
					if (uiTask.UIName.Equals(uiName)) {
						uiTask.UITaskType = UITaskType.Complete;
					}
				}
			}
			
			private void InternalCreateUITask(string uiName, UITaskType uiTaskType, object userData) {
				waitingUITaskQue.Enqueue(UITask.Create(uiName, uiTaskType, userData));
				InternalTryProcessTask();
			}
			
			private bool InternalTryProcessTask() {
				if (waitingUITaskQue.Count == 0) {
					return true;
				}

				var processTaskNum = 0;
				while (waitingUITaskQue.Count > 0 ) {
					var uiTask = waitingUITaskQue.Peek();
					switch (uiTask.UITaskType) {
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

					if (uiTask.UITaskType == UITaskType.Complete || uiTask.UITaskType == UITaskType.None) {
						referencePoolModule.Release(waitingUITaskQue.Dequeue());
						processTaskNum++;
					} else {
						break;
					}
					
					Refresh();
				}
				
				return processTaskNum > 0;
			}

			private void InternalTryOpenUI(UITask uiTask) {
				var uiNode = GetUINode(uiTask.UIName);
				if (uiNode != null) {
					uiNode.Value.UIFormBase.OnOpen(uiTask.UserData);
					uiFormInfoLinked.Remove(uiNode);
					uiFormInfoLinked.AddLast(uiNode);
					uiTask.UITaskType = UITaskType.Complete;
					return;
				}

				var uiObject = uiObjectPool.Spawn(uiTask.UIName);
				if (uiObject == null) {
					return;
				}

				try {
					var uiForm = uiObject.Target.GetComponent<UIFormBase>();
					uiObject.Target.transform.SetParent(groupRoot);
					uiObject.Target.SetActive(true);

					if (!uiForm.IsAlreadyInit) {
						uiForm.OnInit();
					}
					
					uiForm.OnOpen(uiTask.UserData);
					usingUIObject.Add(uiTask.UIName, uiObject);
					uiFormInfoLinked.AddLast(UIFormInfo.Create(uiForm, uiTask.UIName));
					uiTask.UITaskType = UITaskType.Complete;
				}
				catch (Exception e) {
					Debug.LogError(e.Message);
				}
			}

			private void InternalTryCloseUI(UITask uiTask) {
				var uiNode = GetUINode(uiTask.UIName);
				if (uiNode != null) {
					var uiObject = usingUIObject[uiTask.UIName];
					uiObject.Target.SetActive(false);
					uiObjectPool.Recycle(uiObject);
					uiNode.Value.UIFormBase.OnClose();
					uiFormInfoLinked.Remove(uiNode);
					usingUIObject.Remove(uiTask.UIName);
				}
				
				uiTask.UITaskType = UITaskType.Complete;
			}
			
			private void Refresh() {
				var curNode = uiFormInfoLinked.Last;
				var curDepth = uiFormInfoLinked.Count * 100;
				var isTop = true;
				while (curNode != null) {
					if (isTop) {
						if (curNode.Value.IsPause) {
							curNode.Value.UIFormBase.OnResume();
						}
						
						curNode.Value.IsPause = false;
						isTop = false;
					} else {
						if (!curNode.Value.IsPause) {
							curNode.Value.UIFormBase.OnPause();
							curNode.Value.IsPause = true;
						}
						
						if (curNode.Value.Depth != curDepth) {
							curNode.Value.UIFormBase.OnDepthChange();
						}
						
						curNode.Value.Depth = curDepth;
					}

					curNode = curNode.Previous;
				}
			}
			
			private LinkedListNode<UIFormInfo> GetUINode(string uiName) {
				var curNode = uiFormInfoLinked.First;
				while (curNode != null) {
					if (curNode.Value.UIName.Equals(uiName)) {
						break;
					}

					curNode = curNode.Next;
				}

				return curNode;
			}
		}
	}
}