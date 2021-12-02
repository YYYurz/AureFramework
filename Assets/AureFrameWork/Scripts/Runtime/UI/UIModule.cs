//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System.Collections.Generic;
using AureFramework.Event;
using AureFramework.Resource;

namespace AureFramework.UI
{
	public sealed partial class UIModule : AureFrameworkModule
	{
		private Dictionary<string, UIGroup> uiGroupDic = new Dictionary<string,UIGroup>();
		private Queue<UIForm> waitingUIFormQue = new Queue<UIForm>();

		protected override void Awake() {
			base.Awake();
			
			GameMain.GetModule<EventModule>().Subscribe<LoadAssetSuccessEventArgs>(OnLoadAssetSuccess);
			GameMain.GetModule<EventModule>().Subscribe<LoadAssetFailedEventArgs>(OnLoadAssetFailed);
		}

		public override void Tick() {
			
		}

		public override void Clear() {
			GameMain.GetModule<EventModule>().Unsubscribe<LoadAssetFailedEventArgs>(OnLoadAssetFailed);
			GameMain.GetModule<EventModule>().Unsubscribe<LoadAssetFailedEventArgs>(OnLoadAssetFailed);
		}
		
		public void OpenUI(string uiName, object userData) {
			
		}
		
		public void CloseUI(string uiName) {
			
		}

		private void OnLoadAssetSuccess(object sender, GameEventArgs e) {
			
		}
		
		private void OnLoadAssetFailed(object sender, GameEventArgs e) {
			
		}
	}
}