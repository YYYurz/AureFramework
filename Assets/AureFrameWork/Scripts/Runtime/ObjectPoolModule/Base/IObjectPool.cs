﻿//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using Object = UnityEngine.Object;

namespace AureFramework.ObjectPool {
	/// <summary>
	/// 对象池接口
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IObjectPool<T> where T : Object {
		/// <summary>
		/// 获取对象池名称
		/// </summary>
		string Name
		{
			get;
		}
		
		/// <summary>
		/// 获取对象类型
		/// </summary>
		Type ObjectType
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
		/// <param name="isNeed"> 是否需要返回IObject对象 </param>
		/// <param name="name"> 对象名称 </param>
		/// <returns></returns>
		IObject<T> Register(T obj, bool isNeed, string name = null);


		/// <summary>
		/// 获取对象池中任意一个对象
		/// </summary>
		/// <returns></returns>
		IObject<T> Spawn();

		/// <summary>
		/// 获取对象
		/// </summary>
		/// <param name="name"> 对象名称 </param>
		/// <returns></returns>
		IObject<T> Spawn(string name);
		
		/// <summary>
		/// 回收对象
		/// </summary>
		/// <param name="obj"> 对象 </param>
		void Recycle(IObject<T> obj);
		
		/// <summary>
		/// 所有对象加锁
		/// </summary>
		void LockAll();

		/// <summary>
		/// 所有对象解锁
		/// </summary>
		void UnlockAll();

		/// <summary>
		/// 释放所有没有使用中的对象
		/// </summary>
		void ReleaseAllUnused();
	}
}