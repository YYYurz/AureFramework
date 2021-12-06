//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;

namespace AureFramework.ObjectPool {
	/// <summary>
	/// 对象池模块接口
	/// </summary>
	public interface IObjectPoolModule {
		/// <summary>
		/// 获取对象池数量
		/// </summary>
		int Count
		{
			get;
		}

		/// <summary>
		/// 获取对象池
		/// </summary>
		/// <typeparam name="T"> 对象类型 </typeparam>
		/// <returns></returns>
		IObjectPool<T> GetObjectPool<T>() where T : AureObjectBase;

		/// <summary>
		/// 创建对象池
		/// </summary>
		/// <param name="capacity"> 容量 </param>
		/// <param name="expireTime"> 自动释放时间 </param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		IObjectPool<T> CreateObjectPool<T>(int capacity, float expireTime) where T : AureObjectBase;
		
		/// <summary>
		/// 销毁对象池
		/// </summary>
		void DestroyObjectPool<T>(IObjectPool<T> objPool) where T : AureObjectBase;
		
		/// <summary>
		/// 释放所有对象池中未使用的对象
		/// </summary>
		void ReleaseAllUnused();
	}
}