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
	/// <summary>
	/// UI模块
	/// </summary>
	public sealed partial class UIModule : AureFrameworkModule, IUIModule
	{
		private Dictionary<string, UIGroup> uiGroupDic = new Dictionary<string,UIGroup>();
		private Queue<int> loadingTaskIdQue = new Queue<int>();
		private Queue<int> waitingUIFormQue = new Queue<int>();

		public override int Priority => 10;

		public override void Init() {
			Aure.GetModule<IEventModule>().Subscribe<LoadAssetSuccessEventArgs>(OnLoadAssetSuccess);
			Aure.GetModule<IEventModule>().Subscribe<LoadAssetFailedEventArgs>(OnLoadAssetFailed);
		}

		public override void Tick(float elapseTime, float realElapseTime) {
			
		}

		public override void Clear() { 
			Aure.GetModule<IEventModule>().Unsubscribe<LoadAssetFailedEventArgs>(OnLoadAssetFailed);
			Aure.GetModule<IEventModule>().Unsubscribe<LoadAssetFailedEventArgs>(OnLoadAssetFailed);
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