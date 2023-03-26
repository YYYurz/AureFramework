//------------------------------------------------------------
// AureFramework
// Developed By YYYurz
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

namespace AureFramework.Event
{
	/// <summary>
	/// 事件信息
	/// </summary>
	public sealed class EventInfo
	{
		private readonly string eventName;
		private readonly string[] subscriberList;

		public EventInfo(string eventName, string[] subscriberList)
		{
			this.eventName = eventName;
			this.subscriberList = subscriberList;
		}

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
	}
}