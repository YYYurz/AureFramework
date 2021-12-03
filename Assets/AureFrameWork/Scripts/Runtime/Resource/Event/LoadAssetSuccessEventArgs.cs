﻿//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework.Event;
using AureFramework.ReferencePool;

namespace AureFramework.Resource {
	public class LoadAssetSuccessEventArgs : GameEventArgs {
		public string Content
		{
			private set;
			get;
		}

		public static LoadAssetSuccessEventArgs Create(string content) {
			var loadSuccessEventArgs = Aure.GetModule<ReferencePoolModule>().Acquire<LoadAssetSuccessEventArgs>();
			loadSuccessEventArgs.Content = content;
			
			return loadSuccessEventArgs;
		}

		public static void Release(LoadAssetSuccessEventArgs e) {
			if (e == null) {
				return; 
			}
			
			Aure.GetModule<ReferencePoolModule>().Release(e);
		}
		
		public override void Clear() {
			Content = null;
		}
	}
}