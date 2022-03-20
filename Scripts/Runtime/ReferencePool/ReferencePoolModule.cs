//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace AureFramework.ReferencePool
{
	/// <summary>
	/// 引用池模块
	/// </summary>
	public sealed partial class ReferencePoolModule : AureFrameworkModule, IReferencePoolModule
	{
		private static readonly Dictionary<Type, ReferenceCollection> ReferenceCollectionDic = new Dictionary<Type, ReferenceCollection>();

		/// <summary>
		/// 模块初始化，只在第一次被获取时调用一次
		/// </summary>
		public override void Init()
		{
		}

		/// <summary>
		/// 框架轮询
		/// </summary>
		/// <param name="elapseTime"> 距离上一帧的流逝时间，秒单位 </param>
		/// <param name="realElapseTime"> 距离上一帧的真实流逝时间，秒单位 </param>
		public override void Tick(float elapseTime, float realElapseTime)
		{
		}

		/// <summary>
		/// 框架清理
		/// </summary>
		public override void Clear()
		{
			ClearAllReference();
		}

		/// <summary>
		/// 获取所有引用信息
		/// </summary>
		/// <returns></returns>
		public static List<ReferenceInfo> GetReferenceInfoList()
		{
			var referenceInfoList = new List<ReferenceInfo>();

			lock (ReferenceCollectionDic)
			{
				foreach (var referenceCollection in ReferenceCollectionDic)
				{
					var referenceInfo = new ReferenceInfo(referenceCollection.Key.FullName,
						referenceCollection.Value.UnusedReferenceCount, referenceCollection.Value.UsingReferenceCount,
						referenceCollection.Value.AcquireReferenceCount,
						referenceCollection.Value.ReleaseReferenceCount);
					referenceInfoList.Add(referenceInfo);
				}
			}

			return referenceInfoList;
		}

		/// <summary>
		/// 清除所有的引用
		/// </summary>
		public void ClearAllReference()
		{
			lock (ReferenceCollectionDic)
			{
				foreach (var referenceCollection in ReferenceCollectionDic)
				{
					referenceCollection.Value.RemoveAll();
				}

				ReferenceCollectionDic.Clear();
			}
		}

		/// <summary>
		/// 获取引用
		/// </summary>
		/// <typeparam name="T"> 引用类型 </typeparam>
		/// <returns></returns>
		public T Acquire<T>() where T : class, IReference, new()
		{
			return (T) GetReferenceCollection(typeof(T)).Acquire();
		}

		/// <summary>
		/// 增加引用数量
		/// </summary>
		/// <param name="num"> 增加数量 </param>
		/// <typeparam name="T"> 引用类型 </typeparam>
		public void Add<T>(int num) where T : class, IReference, new()
		{
			GetReferenceCollection(typeof(T)).Add(num);
		}

		/// <summary>
		/// 移除引用数量
		/// </summary>
		/// <param name="num"> 移除数量 </param>
		/// <typeparam name="T"> 引用类型 </typeparam>
		public void Remove<T>(int num) where T : class, IReference, new()
		{
			GetReferenceCollection(typeof(T)).Add(num);
		}

		/// <summary>
		/// 移除一个类型的所有引用
		/// </summary>
		/// <typeparam name="T"> 引用类型 </typeparam>
		public void RemoveAll<T>() where T : class, IReference, new()
		{
			GetReferenceCollection(typeof(T)).RemoveAll();
		}

		/// <summary>
		/// 获取引用
		/// </summary>
		/// <param name="referenceType"> 引用类型 </param>
		/// <returns></returns>
		public IReference Acquire(Type referenceType)
		{
			if (!CheckReferenceType(referenceType))
			{
				return null;
			}

			return GetReferenceCollection(referenceType).Acquire();
		}

		/// <summary>
		/// 增加引用数量
		/// </summary>
		/// <param name="referenceType"> 引用类型 </param>
		/// <param name="num"> 增加数量 </param>
		public void Add(Type referenceType, int num)
		{
			if (!CheckReferenceType(referenceType))
			{
				return;
			}

			GetReferenceCollection(referenceType).Add(num);
		}

		/// <summary>
		/// 移除引用数量
		/// </summary>
		/// <param name="referenceType"> 引用类型 </param>
		/// <param name="num"> 移除数量 </param>
		public void Remove(Type referenceType, int num)
		{
			if (!CheckReferenceType(referenceType))
			{
				return;
			}

			GetReferenceCollection(referenceType).Remove(num);
		}

		/// <summary>
		/// 移除一个类型的所有引用
		/// </summary>
		/// <param name="referenceType"> 引用类型 </param>
		public void RemoveAll(Type referenceType)
		{
			if (!CheckReferenceType(referenceType))
			{
				return;
			}

			GetReferenceCollection(referenceType).RemoveAll();
		}

		/// <summary>
		/// 释放引用回池子
		/// </summary>
		/// <param name="reference"></param>
		public void Release(IReference reference)
		{
			if (reference == null)
			{
				Debug.LogError("ReferencePoolModule : Reference is null.");
				return;
			}

			var referenceType = reference.GetType();
			if (!CheckReferenceType(referenceType))
			{
				return;
			}

			GetReferenceCollection(referenceType).Release(reference);
		}

		private static bool CheckReferenceType(Type referenceType)
		{
			if (referenceType == null)
			{
				Debug.LogError("ReferencePoolModule : Reference type is null.");
				return false;
			}

			if (!referenceType.IsClass || referenceType.IsAbstract)
			{
				Debug.LogError("ReferencePoolModule : Reference type is not a non-abstract class.");
				return false;
			}

			if (!typeof(IReference).IsAssignableFrom(referenceType))
			{
				Debug.LogError("ReferencePoolModule : Reference type is not a subclass of IReference.");
				return false;
			}

			return true;
		}

		private static ReferenceCollection GetReferenceCollection(Type referenceType)
		{
			ReferenceCollection referenceCollection;
			lock (ReferenceCollectionDic)
			{
				if (!ReferenceCollectionDic.TryGetValue(referenceType, out referenceCollection))
				{
					referenceCollection = new ReferenceCollection(referenceType);
					ReferenceCollectionDic.Add(referenceType, referenceCollection);
				}
			}

			return referenceCollection;
		}
	}
}