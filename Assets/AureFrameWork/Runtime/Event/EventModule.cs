//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace AureFramework.Event {
	public sealed partial class EventModule : AureFrameworkModule {
		private static readonly Dictionary<int, EventObject> EventDic = new Dictionary<int, EventObject>();
		
		public override int Priority => 2;

		public override void Tick() {
			
		}

		public override void Clear() {
			
		}

		/// <summary>
		/// 订阅事件
		/// </summary>
		/// <param name="eventId"> 事件类型唯一Id </param>
		/// <param name="eventHandler"> 委托函数 </param>
		public void Subscribe(int eventId, EventHandler<GameEventArgs> eventHandler) {
			if (eventHandler == null) {
				Debug.LogError("AureFramework EventModule : EventHandler is null.");
				return;
			}
			
			var eventObject = InternalGetEventObject(eventId);
			eventObject.Subscribe(eventHandler);
		}

		/// <summary>
		/// 取消订阅事件
		/// </summary>
		/// <param name="eventId"> 事件类型唯一Id </param>
		/// <param name="eventHandler"> 委托函数 </param>
		public void Unsubscribe(int eventId, EventHandler<GameEventArgs> eventHandler) {
			if (eventHandler == null) {
				Debug.LogError("AureFramework EventModule : EventHandler is null.");
				return;
			}
			
			var eventObject = InternalGetEventObject(eventId);
			eventObject.Unsubscribe(eventHandler);
		}
		
		/// <summary>
		/// 发送事件
		/// </summary>
		/// <param name="sender"> 发送者实例 </param>
		/// <param name="eventArgs"> 事件信息类 </param>
		public void Fire(object sender, GameEventArgs eventArgs) {
			if (eventArgs == null) {
				Debug.LogError("AureFramework EventModule : EventArgs is null.");
				return;
			}
			
			var eventObject = InternalGetEventObject(eventArgs.EventId);
			eventObject.Fire(sender, eventArgs);
		}

		private static EventObject InternalGetEventObject(int eventId) {
			if (EventDic.TryGetValue(eventId, out var eventObject)) {
				return eventObject;
			}
			
			eventObject = new EventObject();
			EventDic.Add(eventId, eventObject);

			return eventObject;
		}
	}
}