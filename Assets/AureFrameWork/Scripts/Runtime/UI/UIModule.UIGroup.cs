//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
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
			private readonly Dictionary<string, IObject<GameObject>> usingUIObject = new Dictionary<string, IObject<GameObject>>();
			private readonly LinkedList<UIForm> uiFormLinked = new LinkedList<UIForm>();
			private readonly Queue<UITask> waitingUITaskQue = new Queue<UITask>();
			private readonly string groupName;
			private int groupDepth;
			private int curUIDepth;
			private float waitTime;
			private const float taskExpireTime = 1f;

			public UIGroup(IObjectPool<GameObject> uiObjectPool, string groupName, int groupDepth) {
				this.uiObjectPool = uiObjectPool;
				this.groupName = groupName;
				this.groupDepth = groupDepth;
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
			/// 轮询打开的UI
			/// 处理未处理的UI操作
			/// </summary>
			/// <param name="elapseTime"></param>
			public void Update(float elapseTime) {
				waitTime += elapseTime;
				if (waitTime > taskExpireTime) {
					var uiTask = waitingUITaskQue.Dequeue();
					Aure.GetModule<IReferencePoolModule>().Release(uiTask);
					waitTime = 0f;
				} else {
					waitTime = TryProcessTask() ? 0f : waitTime;
				}
				
				foreach (var uiForm in uiFormLinked) {
					uiForm.OnUpdate(elapseTime);
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
			/// 获取已UIForm
			/// </summary>
			/// <param name="uiName"> UI名称 </param>
			/// <returns></returns>
			public UIForm GetUIForm(string uiName) {
				var uiNode = GetUINode(uiName);
				return uiNode?.Value ;
			}

			/// <summary>
			/// 获取所有已打开UIForm
			/// </summary>
			/// <returns></returns>
			public UIForm[] GetAllUIForm() {
				return uiFormLinked.ToArray();
			}

			/// <summary>
			/// 入队打开UI操作
			/// </summary>
			/// <param name="uiName"> UI名称 </param>
			/// <param name="userData"> 用户数据 </param>
			public void OpenUI(string uiName, object userData) {
				waitingUITaskQue.Enqueue(UITask.Create(uiName, UITaskType.OpenUI, userData));
			}

			/// <summary>
			/// 入队关闭UI操作
			/// </summary>
			/// <param name="uiName"> UI名称 </param>
			public void CloseUI(string uiName) {
				waitingUITaskQue.Enqueue(UITask.Create(uiName, UITaskType.CloseUI, null));
			}

			/// <summary>
			/// 清除队列中所有未处理操作，关闭所有已打开UI
			/// </summary>
			public void CloseAllUI() {
				ClearAllWaitingUITask();
				var curNode = uiFormLinked.Last;
				while (curNode != null) {
					waitingUITaskQue.Enqueue(UITask.Create(curNode.Value.UIName, UITaskType.CloseUI, null));
					curNode = curNode.Previous;
					uiFormLinked.RemoveLast();
				}
			}

			/// <summary>
			/// 除了传入UI，清除队列中所有未处理操作，关闭所有已打开UI
			/// </summary>
			/// <param name="uiName"> UI名称 </param>
			public void CloseAllExcept(string uiName) {
				ClearAllWaitingUITask();
				var curNode = uiFormLinked.Last;
				while (curNode != null) {
					var curNodeName = curNode.Value.UIName;
					curNode = curNode.Previous;
					if (!curNodeName.Equals(uiName)) {
						waitingUITaskQue.Enqueue(UITask.Create(curNodeName, UITaskType.CloseUI, null));
						uiFormLinked.RemoveLast();
					}
				}
			}

			/// <summary>
			/// 清除队列中所有未处理的UI操作
			/// </summary>
			public void ClearAllWaitingUITask() {
				waitingUITaskQue.Clear();
			}
			
			/// <summary>
			/// 丢弃队列中UI的操作
			/// </summary>
			/// <param name="uiName"> UI名称 </param>
			public void DiscardUITask(string uiName) {
				foreach (var uiTask in waitingUITaskQue) {
					if (uiTask.UIName.Equals(uiName)) {
						uiTask.Discard();
					}
				}
			}
			
			private bool TryProcessTask() {
				if (waitingUITaskQue.Count == 0) {
					return true;
				}

				var uiTask = waitingUITaskQue.Peek();
				switch (uiTask.UITaskType) {
					case UITaskType.Discard:
						waitingUITaskQue.Dequeue();
						return true;
					case UITaskType.OpenUI:
						return TryOpenUI(uiTask.UIName, uiTask.UserData);
					case UITaskType.CloseUI:
						return TryCloseUI(uiTask.UIName);
				}
				
				Refresh();
				return true;
			}

			private bool TryOpenUI(string uiName, object userData) {
				var uiNode = GetUINode(uiName);
				if (uiNode != null) {
					uiNode.Value.OnOpen(userData);
					uiFormLinked.Remove(uiNode);
					uiFormLinked.AddLast(uiNode);
					waitingUITaskQue.Dequeue();
					return true;
				}

				var uiObject = uiObjectPool.Spawn(uiName);
				if (uiObject == null) {
					return false;
				}

				var uiForm = uiObject.Target.GetComponent<UIForm>();
				uiForm.OnOpen(userData);
				uiFormLinked.AddLast(uiForm);
				usingUIObject.Add(uiName, uiObject);
				waitingUITaskQue.Dequeue();
				return true;
			}

			private bool TryCloseUI(string uiName) {
				var uiNode = GetUINode(uiName);
				if (uiNode != null) {
					var uiObject = usingUIObject[uiName];
					uiObjectPool.Recycle(uiObject);
					uiNode.Value.OnClose();
					uiFormLinked.Remove(uiNode);
					usingUIObject.Remove(uiName);
					waitingUITaskQue.Dequeue();
					return true;
				}

				return false;
			}
			
			private void Refresh() {
				
			}
			
			private LinkedListNode<UIForm> GetUINode(string uiName) {
				var curNode = uiFormLinked.First;
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