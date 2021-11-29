﻿//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework.Event;
using AureFramework.ReferencePool;

namespace AureFramework.Resource {
	public class LoadFailedEventArgs : GameEventArgs {
		public string Content
		{
			private set;
			get;
		}

		public static LoadFailedEventArgs Create(string content) {
			var loadSuccessEventArgs = GameMain.GetModule<ReferencePoolModule>().Acquire<LoadFailedEventArgs>();
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