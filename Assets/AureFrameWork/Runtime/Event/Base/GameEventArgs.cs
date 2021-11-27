//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using AureFramework.ReferencePool;

namespace AureFramework.Event {
	public abstract class GameEventArgs : EventArgs, IReference {
		/// <summary>
		/// 事件类唯一Id
		/// </summary>
		public abstract int EventId
		{
			get;
		}

		/// <summary>
		/// 清理函数
		/// </summary>
		public abstract void Clear();
	}
}