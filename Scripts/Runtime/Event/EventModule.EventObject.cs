//------------------------------------------------------------
// AureFramework
// Developed By YYYurz
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using System.Threading;
using AureFramework.ReferencePool;

namespace AureFramework.Event
{
	public sealed partial class EventModule : AureFrameworkModule, IEventModule
	{
		/// <summary>
		/// 内部事件对象
		/// </summary>
		private class EventObject
		{
			private event EventHandler<AureEventArgs> EventArgs;
			private bool isHasSubscriber;
			private string[] subscriberList;

			public bool IsHaveSubscriber
			{
				get
				{
					if (EventArgs == null)
					{
						return false;
					}

					if (EventArgs.GetInvocationList().Length == 0)
					{
						return false;
					}

					return true;
				}
			}

			public string[] SubscriberList
			{
				get
				{
					Volatile.Read(ref EventArgs);

					if (EventArgs != null)
					{
						var invocationList = EventArgs.GetInvocationList();
						var length = invocationList.Length;
						subscriberList = new string[length];

						for (var i = 0; i < length; i++)
						{
							var invocation = invocationList[i];
							subscriberList[i] = $"{invocation.Target}.{invocation.Method.Name}(";
							var parameterList = invocation.Method.GetParameters();
							for (var k = 0; k < parameterList.Length; k++)
							{
								subscriberList[i] += k == 0 ? parameterList[k].ToString() : ", " + parameterList[k];
							}

							subscriberList[i] += ")";
						}

						return subscriberList;
					}

					return null;
				}
			}

			public void Fire(object sender, AureEventArgs e)
			{
				Volatile.Read(ref EventArgs);
				EventArgs?.Invoke(sender, e);
				Aure.GetModule<IReferencePoolModule>().Release(e);
			}

			public void Subscribe(EventHandler<AureEventArgs> e)
			{
				EventArgs += e;
			}

			public void Unsubscribe(EventHandler<AureEventArgs> e)
			{
				EventArgs -= e;
			}
		}
	}
}