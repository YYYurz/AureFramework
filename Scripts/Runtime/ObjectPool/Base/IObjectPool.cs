//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

namespace AureFramework.ObjectPool
{
	/// <summary>
	/// 对象池接口
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IObjectPool<T> where T : ObjectBase
	{
		/// <summary>
		/// 获取对象池名称
		/// </summary>
		string Name
		{
			get;
		}

		/// <summary>
		/// 获取使用中对象的数量
		/// </summary>
		int UsingCount
		{
			get;
		}

		/// <summary>
		/// 获取未使用对象的数量
		/// </summary>
		int UnusedCount
		{
			get;
		}

		/// <summary>
		/// 获取或设置对象池容量
		/// </summary>
		int Capacity
		{
			get;
			set;
		}

		/// <summary>
		/// 获取或设置对象池过期秒数
		/// </summary>
		float ExpireTime
		{
			get;
			set;
		}

		/// <summary>
		/// 注册一个新创建的对象
		/// </summary>
		/// <param name="obj"> 对象 </param>
		/// <param name="isNeed"> 是否需要使用注册后的对象 </param>
		/// <param name="objName"> 对象名称 </param>
		void Register(T obj, bool isNeed, string objName = null);

		/// <summary>
		/// 是否能获取对象
		/// </summary>
		/// <param name="objName"> 对象名称 </param>
		/// <returns></returns>
		bool CanSpawn(string objName);

		/// <summary>
		/// 获取对象池中任意一个对象
		/// </summary>
		/// <returns></returns>
		T Spawn();

		/// <summary>
		/// 获取对象
		/// </summary>
		/// <param name="objName"> 对象名称 </param>
		/// <returns></returns>
		T Spawn(string objName);

		/// <summary>
		/// 回收对象
		/// </summary>
		/// <param name="obj"> 对象 </param>
		void Recycle(T obj);

		/// <summary>
		/// 所有对象加锁
		/// </summary>
		void LockAll();

		/// <summary>
		/// 所有对象解锁
		/// </summary>
		void UnlockAll();

		/// <summary>
		/// 对象加锁（同名所有对象）
		/// </summary>
		/// <param name="objName"> 对象名称 </param>
		void Lock(string objName);

		/// <summary>
		/// 对象解锁（同名所有对象）
		/// </summary>
		/// <param name="objName"> 对象名称 </param>
		void Unlock(string objName);

		/// <summary>
		/// 释放所有没有使用中的对象
		/// </summary>
		void ReleaseAllUnused();

		/// <summary>
		/// 对象是否存在
		/// </summary>
		/// <param name="objName"> 对象名称 </param>
		/// <returns></returns>
		bool IsHasObject(string objName);
	}
}