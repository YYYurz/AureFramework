//------------------------------------------------------------
// AureFramework
// Developed By YYYurz
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework.ReferencePool;

namespace AureFramework.ObjectPool
{
	/// <summary>
	/// 对象基类
	/// </summary>
	public abstract class ObjectBase : IReference
	{
		/// <summary>
		/// 获取对象名称
		/// </summary>
		public string Name
		{
			get;
			protected set;
		}

		/// <summary>
		/// 获取或设置对象是否加锁
		/// </summary>
		public bool IsLock
		{
			get;
			set;
		}

		/// <summary>
		/// 清理对象
		/// </summary>
		public virtual void Clear()
		{
			Name = null;
			IsLock = false;
		}

		/// <summary>
		/// 获取对象时触发
		/// </summary>
		public virtual void OnSpawn()
		{
		}

		/// <summary>
		/// 回收对象时触发
		/// </summary>
		public virtual void OnRecycle()
		{
		}

		/// <summary>
		/// 释放对象时触发
		/// </summary>
		public virtual void OnRelease()
		{
		}
	}
}