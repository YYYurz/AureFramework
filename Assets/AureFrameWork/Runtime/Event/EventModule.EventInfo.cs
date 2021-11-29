//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

namespace AureFramework.Event {
	public sealed partial class EventModule {
		public sealed class EventInfo {
			private readonly string eventName;
			private readonly string[] subscriberList;

			public string EventName
			{
				get
				{
					return eventName;
				}
			}

			public string[] SubscriberList
			{
				get
				{
					return subscriberList;	
				}
			}

			public EventInfo(string eventName, string[] subscriberList) {
				this.eventName = eventName;
				this.subscriberList = subscriberList;
			}
		}
	}
}