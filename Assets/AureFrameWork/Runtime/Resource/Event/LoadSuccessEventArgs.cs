//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using AureFramework.Event;
using AureFramework.ReferencePool;

namespace AureFramework.Resource {
	public class LoadSuccessEventArgs : GameEventArgs {
		private static readonly int EventId = typeof(LoadSuccessEventArgs).GetHashCode();

		public static LoadSuccessEventArgs Create() {
			var loadSuccessEventArgs = GameMain.GetModule<ReferencePoolModule>().Acquire<LoadSuccessEventArgs>();
			return loadSuccessEventArgs;
		}
	}
}