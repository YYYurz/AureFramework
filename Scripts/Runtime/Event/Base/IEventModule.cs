//------------------------------------------------------------
// AureFramework
// Developed By YYYurz
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;

namespace AureFramework.Event
{
	/// <summary>
	/// 事件模块接口
	/// </summary>
	public interface IEventModule
	{
		/// <summary>
		/// 订阅事件
		/// </summary>
		/// <param name="eventHandler"> 委托函数 </param>
		void Subscribe<T>(EventHandler<AureEventArgs> eventHandler) where T : AureEventArgs;

		/// <summary>
		/// 取消订阅事件
		/// </summary>
		/// <param name="eventHandler"> 委托函数 </param>
		void Unsubscribe<T>(EventHandler<AureEventArgs> eventHandler) where T : AureEventArgs;

		/// <summary>
		/// 发送事件
		/// </summary>
		/// <param name="sender"> 发送者实例 </param>
		/// <param name="eventArgs"> 事件信息类 </param>
		void Fire(object sender, AureEventArgs eventArgs);
	}
}