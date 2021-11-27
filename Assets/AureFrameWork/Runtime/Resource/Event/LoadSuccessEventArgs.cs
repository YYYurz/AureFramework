//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework.Event;
using AureFramework.ReferencePool;

namespace AureFramework.Resource {
	public class LoadSuccessEventArgs : GameEventArgs {
		public static readonly int Id = typeof(LoadSuccessEventArgs).GetHashCode();
		
		public override int EventId
		{
			get
			{
				return Id;
			}
		}

		public string Content
		{
			private set;
			get;
		}

		public static LoadSuccessEventArgs Create(string content) {
			var loadSuccessEventArgs = GameMain.GetModule<ReferencePoolModule>().Acquire<LoadSuccessEventArgs>();
			loadSuccessEventArgs.Content = content;
			
			return loadSuccessEventArgs;
		}

		public static void Release(LoadSuccessEventArgs e) {
			if (e == null) {
				return; 
			}
			
			GameMain.GetModule<ReferencePoolModule>().Release(e);
		}
		
		public override void Clear() {
			Content = null;
		}
	}
}