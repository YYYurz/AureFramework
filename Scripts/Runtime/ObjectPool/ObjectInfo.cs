//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;

namespace AureFramework.ObjectPool
{
	/// <summary>
	/// 对象信息
	/// </summary>
	public sealed class ObjectInfo
	{
		private readonly string name;
		private readonly DateTime lastUseTime;
		private readonly bool isLock;
		private readonly bool isInUse;

		public ObjectInfo(string name, DateTime lastUseTime, bool isLock, bool isInUse)
		{
			this.name = name;
			this.lastUseTime = lastUseTime;
			this.isLock = isLock;
			this.isInUse = isInUse;
		}

		/// <summary>
		/// 获取对象上一次使用时间
		/// </summary>
		public DateTime LastUseTime
		{
			get
			{
				return lastUseTime;
			}
		}

		/// <summary>
		/// 获取对象名称
		/// </summary>
		public string Name
		{
			get
			{
				return name;
			}
		}

		/// <summary>
		/// 获取对象是否加锁
		/// </summary>
		public bool IsLock
		{
			get
			{
				return isLock;
			}
		}

		/// <summary>
		/// 获取对象是否正在被使用
		/// </summary>
		public bool IsInUse
		{
			get
			{
				return isInUse;
			}
		}
	}
}