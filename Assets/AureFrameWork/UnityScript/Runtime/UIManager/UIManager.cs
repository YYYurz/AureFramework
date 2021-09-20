//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework.UI;
using UnityEngine;

namespace AureFramework.Runtime.UI {
	public class UIManager : AureFrameworkManager {
		private IUIModule uiModule;

		protected override void Awake() {
			base.Awake();

			uiModule = GameMain.GetModule<UIModule>();
			if (uiModule == null) {
				Debug.LogError("UIManager : UIModule is invalid");
				return;
			}
		}
	}
}