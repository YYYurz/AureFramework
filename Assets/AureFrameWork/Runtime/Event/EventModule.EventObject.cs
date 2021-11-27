//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;

namespace AureFramework.Event {
	public sealed partial class EventModule {
		private class EventObject {
			private event EventHandler<GameEventArgs> EventArgs;

			public void Fire(object sender, GameEventArgs e) {
				if (EventArgs == null) {
					return;
				}

				if (e == null) {
					return;
				}
				
				EventArgs.Invoke(sender, e);
			}
			
			public void Subscribe(EventHandler<GameEventArgs> e) {
				EventArgs += e;
			}

			public void Unsubscribe(EventHandler<GameEventArgs> e) {
				EventArgs -= e;
			}
		}
	}
}