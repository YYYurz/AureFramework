//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using AureFramework.ReferencePool;

namespace AureFramework.ObjectPool {
	public abstract class ObjectBase : IReference {
		/// <summary>
		/// 获取对象唯一Id
		/// </summary>
		public abstract int TargetId
		{
			get;
		}
		
		/// <summary>
		/// 获取或设置对象上次使用时间。
		/// </summary>
		public abstract DateTime LastUseTime
		{
			get;
			set;
		}

		/// <summary>
		/// 获取对象名称
		/// </summary>
		public abstract string Name
		{
			get;
		}
		
		/// <summary>
		/// 获取或设置对象是否加锁
		/// </summary>
		public abstract bool IsLock
		{
			get;
			set;
		}

		/// <summary>
		/// 设置或获取对象是否正在被使用
		/// </summary>
		public abstract bool IsInUse
		{
			get;
			set;
		}

		/// <summary>
		/// 获取对象信息
		/// </summary>
		/// <returns></returns>
		public abstract ObjectInfo GetObjectInfo();

		/// <summary>
		/// 获取对象时触发
		/// </summary>
		public abstract void OnSpawn();

		/// <summary>
		/// 回收对象时触发
		/// </summary>
		public abstract void OnRecycle();

		/// <summary>
		/// 释放对象时触发
		/// </summary>
		public abstract void OnRelease();

		/// <summary>
		/// 清理对象
		/// </summary>
		public abstract void Clear();
	}
}