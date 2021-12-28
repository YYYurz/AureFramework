//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace AureFramework.Event {
	public sealed partial class EventModule : AureFrameworkModule, IEventModule {
		private static readonly Dictionary<Type, EventObject> EventObjectDic = new Dictionary<Type, EventObject>();
		
		public override int Priority => 2;

		public override void Init() {
			
		}

		public override void Tick(float elapseTime, float realElapseTime) {
			
		}

		public override void Clear() {
			
		}

		public static List<EventInfo> GetEventInfoList() {
			var eventInfoList = new List<EventInfo>();
			
			lock (EventObjectDic) {
				foreach (var eventObject in EventObjectDic) {
					if (!eventObject.Value.IsHaveSubscriber) {
						continue;
					}
					var referenceInfo = new EventInfo(eventObject.Key.FullName, eventObject.Value.SubscriberList);
					eventInfoList.Add(referenceInfo);
				}
			}

			return eventInfoList;
		}

		/// <summary>
		/// 订阅事件
		/// </summary>
		/// <param name="eventHandler"> 委托函数 </param>
		public void Subscribe<T>(EventHandler<AureEventArgs> eventHandler) where T : AureEventArgs{
			if (eventHandler == null) {
				Debug.LogError("EventModule : EventHandler is null.");
				return;
			}
			
			InternalGetEventObject(typeof(T)).Subscribe(eventHandler);
		}

		/// <summary>
		/// 取消订阅事件
		/// </summary>
		/// <param name="eventHandler"> 委托函数 </param>
		public void Unsubscribe<T>(EventHandler<AureEventArgs> eventHandler) where T : AureEventArgs{
			if (eventHandler == null) {
				Debug.LogError("EventModule : EventHandler is null.");
				return;
			}
			
			InternalGetEventObject(typeof(T)).Unsubscribe(eventHandler);
		}
		
		/// <summary>
		/// 发送事件
		/// </summary>
		/// <param name="sender"> 发送者实例 </param>
		/// <param name="eventArgs"> 事件信息类 </param>
		public void Fire(object sender, AureEventArgs eventArgs) {
			if (eventArgs == null) {
				Debug.LogError("EventModule : EventArgs is null.");
				return;
			}
			
			InternalGetEventObject(eventArgs.GetType()).Fire(sender, eventArgs);
		}

		private static EventObject InternalGetEventObject(Type type) {
			lock (EventObjectDic) {
				if (EventObjectDic.TryGetValue(type, out var eventObject)) {
					return eventObject;
				}
			
				eventObject = new EventObject();
				EventObjectDic.Add(type, eventObject);
				
				return eventObject;
			}
		}
	}
}