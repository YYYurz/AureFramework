//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

namespace AureFramework.ObjectPool {
	public interface IObjectPool {
		/// <summary>
		/// 使用中对象的数量
		/// </summary>
		int UsingCount
		{
			get;
		}

		/// <summary>
		/// 未使用对象的数量
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
		/// 获取对象池中任意一个对象
		/// </summary>
		/// <returns></returns>
		AureObjectBase Spawn();

		/// <summary>
		/// 获取对象
		/// </summary>
		/// <param name="name"> 对象名称 </param>
		/// <returns></returns>
		AureObjectBase Spawn(string name);
		
		/// <summary>
		/// 无名称回收对象，通过无参Spawn函数获取，或者通过名称Default获取
		/// </summary>
		/// <param name="obj"> 对象 </param>
		void Recycle(AureObjectBase obj);

		/// <summary>
		/// 回收对象
		/// </summary>
		/// <param name="name"> 对象名称 </param>
		/// <param name="obj"> 对象 </param>
		void Recycle(string name, AureObjectBase obj);
		
		/// <summary>
		/// 对象加锁
		/// </summary>
		/// <param name="name"> 对象名称 </param>
		void SetLock(string name);

		/// <summary>
		/// 对象解锁
		/// </summary>
		/// <param name="name"> 对象名称 </param>
		void SetUnlock(string name);
		
		/// <summary>
		/// 所有对象加锁
		/// </summary>
		void LockAll();

		/// <summary>
		/// 所有对象解锁
		/// </summary>
		void UnlockAll();

		/// <summary>
		/// 释放所有对象
		/// </summary>
		void ReleaseAll();

		/// <summary>
		/// 释放没有使用中的对象
		/// </summary>
		void ReleaseAllUnused();

		/// <summary>
		/// 轮询
		/// </summary>
		/// <param name="elapseTime"> 距离上一帧的流逝时间，秒单位 </param>
		/// <param name="realElapseTime"> 距离上一帧的真实流逝时间，秒单位 </param>
		void Update(float elapseTime, float realElapseTime);

		/// <summary>
		/// 销毁
		/// </summary>
		void ShutDown();
	}
}