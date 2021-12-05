//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using AureFramework.ReferencePool;

namespace AureFramework.Event {
	public abstract class AureEventArgs : EventArgs, IReference {
		/// <summary>
		/// 清理函数
		/// </summary>
		public abstract void Clear();
	}
}