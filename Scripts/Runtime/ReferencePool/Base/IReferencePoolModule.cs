//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;

namespace AureFramework.ReferencePool
{
	/// <summary>
	/// 引用模块接口
	/// </summary>
	public interface IReferencePoolModule
	{
		/// <summary>
		/// 清除所有的引用
		/// </summary>
		void ClearAllReference();

		/// <summary>
		/// 获取引用
		/// </summary>
		/// <typeparam name="T"> 引用类型 </typeparam>
		/// <returns></returns>
		T Acquire<T>() where T : class, IReference, new();

		/// <summary>
		/// 增加引用数量
		/// </summary>
		/// <param name="num"> 增加数量 </param>
		/// <typeparam name="T"> 引用类型 </typeparam>
		void Add<T>(int num) where T : class, IReference, new();

		/// <summary>
		/// 移除引用数量
		/// </summary>
		/// <param name="num"> 移除数量 </param>
		/// <typeparam name="T"> 引用类型 </typeparam>
		void Remove<T>(int num) where T : class, IReference, new();

		/// <summary>
		/// 移除一个类型的所有引用
		/// </summary>
		/// <typeparam name="T"> 引用类型 </typeparam>
		void RemoveAll<T>() where T : class, IReference, new();

		/// <summary>
		/// 获取引用
		/// </summary>
		/// <param name="referenceType"> 引用类型 </param>
		/// <returns></returns>
		IReference Acquire(Type referenceType);

		/// <summary>
		/// 增加引用数量
		/// </summary>
		/// <param name="referenceType"> 引用类型 </param>
		/// <param name="num"> 增加数量 </param>
		void Add(Type referenceType, int num);

		/// <summary>
		/// 移除引用数量
		/// </summary>
		/// <param name="referenceType"> 引用类型 </param>
		/// <param name="num"> 移除数量 </param>
		void Remove(Type referenceType, int num);

		/// <summary>
		/// 移除一个类型的所有引用
		/// </summary>
		/// <param name="referenceType"> 引用类型 </param>
		void RemoveAll(Type referenceType);

		/// <summary>
		/// 释放引用回池子
		/// </summary>
		/// <param name="reference"></param>
		void Release(IReference reference);
	}
}