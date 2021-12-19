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
			private const float taskExpireTime = 1f;

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
			/// <param name="elapseTime"></param>
			public void Update(float elapseTime) {
				waitTime += elapseTime;
				if (waitTime > taskExpireTime) {
					var uiTask = waitingUITaskQue.Dequeue();
					referencePoolModule.Release(uiTask);
					waitTime = 0f;
				} else {
					waitTime = InternalTryProcessTask() ? 0f : waitTime;
				}
				
				foreach (var uiFormInfo in uiFormInfoLinked) {
					uiFormInfo.FormBase.OnUpdate(elapseTime);
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
				return uiNode?.Value.FormBase;
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
					result[index] = curNode.Value.FormBase;
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
						uiTask.Discard();
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
				while (waitingUITaskQue.Count > 0) {
					var uiTask = waitingUITaskQue.Peek();
					switch (uiTask.UITaskType) {
						case UITaskType.Discard:
							waitingUITaskQue.Dequeue();
							processTaskNum++;
							break;
						case UITaskType.OpenUI:
							processTaskNum += InternalTryOpenUI(uiTask.UIName, uiTask.UserData) ? 1 : 0;
							break;
						case UITaskType.CloseUI:
							InternalTryCloseUI(uiTask.UIName);
							processTaskNum++;
							break;
						
					}
				}
				
				Refresh();
				return processTaskNum > 0;
			}

			private bool InternalTryOpenUI(string uiName, object userData) {
				var uiNode = GetUINode(uiName);
				if (uiNode != null) {
					uiNode.Value.FormBase.OnOpen(userData);
					uiFormInfoLinked.Remove(uiNode);
					uiFormInfoLinked.AddLast(uiNode);
					referencePoolModule.Release(waitingUITaskQue.Dequeue());
					return true;
				}

				var uiObject = uiObjectPool.Spawn(uiName);
				if (uiObject == null) {
					return false;
				}

				try {
					var uiForm = uiObject.Target.GetComponent<UIFormBase>();
					uiObject.Target.transform.SetParent(groupRoot);
					uiForm.OnOpen(userData);
					usingUIObject.Add(uiName, uiObject);
					uiFormInfoLinked.AddLast(UIFormInfo.Create(uiForm, uiName));
				}
				catch (Exception e) {
					Debug.LogError(e.Message);
				}
				
				return true;
			}

			private void InternalTryCloseUI(string uiName) {
				var uiNode = GetUINode(uiName);
				if (uiNode != null) {
					var uiObject = usingUIObject[uiName];
					uiObjectPool.Recycle(uiObject);
					uiNode.Value.FormBase.OnClose();
					uiFormInfoLinked.Remove(uiNode);
					usingUIObject.Remove(uiName);
					referencePoolModule.Release(waitingUITaskQue.Dequeue());
				}
			}
			
			private void Refresh() {
				var curNode = uiFormInfoLinked.Last;
				var curDepth = uiFormInfoLinked.Count * 100;
				var isTop = true;
				while (curNode != null) {
					if (isTop) {
						if (curNode.Value.IsPause) {
							curNode.Value.FormBase.OnResume();
						}
						
						curNode.Value.IsPause = false;
						isTop = false;
					} else {
						if (!curNode.Value.IsPause) {
							curNode.Value.FormBase.OnPause();
							curNode.Value.IsPause = true;
						}
						
						if (curNode.Value.Depth != curDepth) {
							curNode.Value.FormBase.OnDepthChange();
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