//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System.Collections.Generic;

namespace AureFramework.UI {
	public sealed partial class UIModule : AureFrameworkModule, IUIModule {
		/// <summary>
		/// UI组
		/// </summary>
		private sealed partial class UIGroup {
			private readonly UIModule uiModule;
			private readonly List<UIForm> uiFormList = new List<UIForm>();
			private readonly Queue<string> waitingTaskQue = new Queue<string>();
			private readonly string groupName;
			private int groupDepth;
			private int curUIDepth;

			public UIGroup(UIModule uiModule, string groupName, int groupDepth) {
				this.uiModule = uiModule;
				this.groupName = groupName;
				this.groupDepth = groupDepth;
			}
			
			public void Update(float elapseTime) {
				foreach (var uiForm in uiFormList) {
					uiForm.OnUpdate(elapseTime);
				}
			}

			// public bool HasUIForm(string uiName) {
			// 	
			// }

			// public UIForm GetUIForm(string uiName) {
			// 	
			// }

			public UIForm[] GetAllUIForm() {
				return uiFormList.ToArray();
			}

			public void OpenUIForm(string uiName) {

			}

			public void CloseUIForm() {

			}

			public void AddUIQueue(string uiName) {
				waitingOpenUIQue.Enqueue(uiName);
			}

			private void Refresh() {
				
			}

			private void ProcessOpenUIQueue() {
				if (waitingOpenUIQue.Count == 0) {
					return;
				}
				
				
			}
		}
	}
}