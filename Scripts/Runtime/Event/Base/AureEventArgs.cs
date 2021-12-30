//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using AureFramework.ReferencePool;

namespace AureFramework.Event
{
	/// <summary>
	/// 事件基类
	/// </summary>
	public abstract class AureEventArgs : EventArgs, IReference
	{
		/// <summary>
		/// 清理函数
		/// </summary>
		public abstract void Clear();
	}
}