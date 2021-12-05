//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework.Event;
using AureFramework.ReferencePool;

namespace AureFramework.Resource {
	public class LoadAssetFailedEventArgs : AureEventArgs {
		public string Content
		{
			private set;
			get;
		}

		public static LoadAssetFailedEventArgs Create(string content) {
			var loadSuccessEventArgs = Aure.GetModule<IReferencePoolModule>().Acquire<LoadAssetFailedEventArgs>();
			loadSuccessEventArgs.Content = content;
			
			return loadSuccessEventArgs;
		}

		public static void Release(LoadAssetSuccessEventArgs e) {
			if (e == null) {
				return; 
			}
			
			Aure.GetModule<IReferencePoolModule>().Release(e);
		}
		
		public override void Clear() {
			Content = null;
		}
	}
}