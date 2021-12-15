//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

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
		private sealed partial class UIGroup {
			private readonly IObjectPool<GameObject> uiObjectPool;
			private readonly LinkedList<UIForm> uiFormLinked = new LinkedList<UIForm>();
			private readonly Queue<UITask> waitingTaskQue = new Queue<UITask>();
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
			/// 轮询打开的UI
			/// 处理未处理的UI操作
			/// </summary>
			/// <param name="elapseTime"></param>
			public void Update(float elapseTime) {
				waitTime += elapseTime;
				if (waitTime > taskExpireTime) {
					var uiTask = waitingTaskQue.Dequeue();
					Aure.GetModule<IReferencePoolModule>().Release(uiTask);
					waitTime = 0f;
				} else {
					waitTime = TryProcessTask() ? 0f : waitTime;
				}
				
				foreach (var uiForm in uiFormLinked) {
					uiForm.OnUpdate(elapseTime);
				}
			}

			public string GroupName
			{
				get
				{
					return groupName;
				}
			}

			public bool IsHasUI(string uiName) {
				foreach (var uiForm in uiFormLinked) {
					if (uiForm.UIName.Equals(uiName)) {
						return true;
					}
				}
				
				return false;
			}

			public UIForm GetUIForm(string uiName) {
				foreach (var uiForm in uiFormLinked) {
					if (uiForm.UIName.Equals(uiName)) {
						return uiForm;
					}
				}
				
				return null;
			}

			public UIForm[] GetAllUIForm() {
				return uiFormLinked.ToArray();
			}

			public void OpenUI(string uiName) {
				waitingTaskQue.Enqueue(UITask.Create(uiName, UITaskType.OpenUI));
			}

			public void CloseUI(string uiName) {
				waitingTaskQue.Enqueue(UITask.Create(uiName, UITaskType.CloseUI));
			}

			public void CloseAllUI() {
				waitingTaskQue.Clear();
				var curNode = uiFormLinked.Last;
				while (curNode != null) {
					waitingTaskQue.Enqueue(UITask.Create(curNode.Value.UIName, UITaskType.CloseUI));
					curNode = curNode.Previous;
					uiFormLinked.RemoveLast();
				}
			}

			public void CloseAllExcept(string uiName) {
				waitingTaskQue.Clear();
				var curNode = uiFormLinked.Last;
				while (curNode != null) {
					var curNodeName = curNode.Value.UIName;
					curNode = curNode.Previous;
					if (!curNodeName.Equals(uiName)) {
						waitingTaskQue.Enqueue(UITask.Create(curNodeName, UITaskType.CloseUI));
						uiFormLinked.RemoveLast();
					}
				}
			}
			
			private bool TryProcessTask() {
				if (waitingTaskQue.Count == 0) {
					return true;
				}

				var task = waitingTaskQue.Peek();
				
				
				Refresh();
				return true;
			}
			
			private void Refresh() {
				
			}
		}
	}
}