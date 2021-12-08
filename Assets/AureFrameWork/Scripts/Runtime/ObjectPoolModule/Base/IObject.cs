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
	/// 对象接口
	/// </summary>
	public interface IObject<T> where T : Object {
		/// <summary>
		/// 获取对象
		/// </summary>
		T Target
		{
			get;
		}

		/// <summary>
		/// 获取对象上一次使用时间
		/// </summary>
		DateTime LastUseTime
		{
			get;
		}

		/// <summary>
		/// 获取对象名称
		/// </summary>
		string Name
		{
			get;
		}

		/// <summary>
		/// 获取或设置对象是否加锁
		/// </summary>
		bool IsLock
		{
			get;
			set;
		}

		/// <summary>
		/// 注册对象获取时触发回调
		/// </summary>
		/// <param name="callBack"> 回调方法 </param>
		void RegisterOnSpawn(Action callBack);
		
		/// <summary>
		/// 注册对象回收时触发回调
		/// </summary>
		/// <param name="callBack"> 回调方法 </param>
		void RegisterOnRecycle(Action callBack);
		
		/// <summary>
		/// 注册对象释放时触发回调
		/// </summary>
		/// <param name="callBack"> 回调方法 </param>
		void RegisterOnRelease(Action callBack);
	}
}