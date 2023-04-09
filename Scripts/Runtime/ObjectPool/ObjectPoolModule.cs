//------------------------------------------------------------
// AureFramework
// Developed By YYYurz
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace AureFramework.ObjectPool
{
	/// <summary>
	/// 对象池模块
	/// </summary>
	public sealed partial class ObjectPoolModule : AureFrameworkModule, IObjectPoolModule
	{
		private static readonly List<ObjectPoolBase> ObjectPoolList = new List<ObjectPoolBase>();

		/// <summary>
		/// 获取对象池数量
		/// </summary>
		public int Count
		{
			get
			{
				return ObjectPoolList.Count;
			}
		}

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
			foreach (var objectPool in ObjectPoolList)
			{
				objectPool.Update(elapseTime, realElapseTime);
			}
		}

		/// <summary>
		/// 框架清理
		/// </summary>
		public override void Clear()
		{
			InternalDestroyAllObjectPool();
		}

		/// <summary>
		/// 获取所有对象池信息
		/// </summary>
		/// <returns></returns>
		public static ObjectPoolInfo[] GetAllObjectPoolInfo()
		{
			var objectPoolInfos = new ObjectPoolInfo[ObjectPoolList.Count];
			for (var i = 0; i < ObjectPoolList.Count; i++)
			{
				objectPoolInfos[i] = ObjectPoolList[i].GetObjectPoolInfo();
			}

			return objectPoolInfos;
		}

		/// <summary>
		/// 获取对象池
		/// </summary>
		/// <param name="poolName"> 对象池名称 </param>
		/// <typeparam name="T"> 对象类型 </typeparam>
		/// <returns></returns>
		public IObjectPool<T> GetObjectPool<T>(string poolName) where T : ObjectBase
		{
			TryGetObjectPool<T>(poolName, out var objectPool);
			return (IObjectPool<T>) objectPool;
		}

		/// <summary>
		/// 创建对象池
		/// </summary>
		/// <param name="poolName"> 对象池名称 </param>
		/// <param name="capacity"> 容量 </param>
		/// <param name="expireTime"> 自动释放时间 </param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public IObjectPool<T> CreateObjectPool<T>(string poolName, int capacity, float expireTime) where T : ObjectBase
		{
			return (IObjectPool<T>) InternalCreateObjectPool<T>(poolName, capacity, expireTime);
		}

		/// <summary>
		/// 销毁对象池
		/// <param name="poolName"> 对象池名称 </param>
		/// </summary>
		public void DestroyObjectPool<T>(string poolName, IObjectPool<T> objPool) where T : ObjectBase
		{
			InternalDestroyObjectPool(poolName, objPool);
		}

		/// <summary>
		/// 释放所有对象池中未使用的对象
		/// </summary>
		public void ReleaseAllUnused()
		{
			foreach (var objectPool in ObjectPoolList)
			{
				objectPool.ReleaseAllUnused();
			}
		}

		private ObjectPoolBase InternalCreateObjectPool<T>(string poolName, int capacity, float expireTime) where T : ObjectBase
		{
			if (TryGetObjectPool<T>(poolName, out var objectPool))
			{
				Debug.LogError($"ObjectPoolModule : Object Pool is already exists, object type : {objectPool.ObjectType.FullName}.");
				return null;
			}

			objectPool = new ObjectPool<T>(poolName, capacity, expireTime);
			ObjectPoolList.Add(objectPool);
			return objectPool;
		}

		private static void InternalDestroyObjectPool<T>(string poolName, IObjectPool<T> objPool) where T : ObjectBase
		{
			if (objPool == null || !TryGetObjectPool<T>(poolName, out var objectPool))
			{
				Debug.LogError($"ObjectPoolModule : Object Pool is invalid.");
				return;
			}

			objectPool.ShutDown();
			ObjectPoolList.Remove(objectPool);
		}

		private void InternalDestroyAllObjectPool()
		{
			foreach (var objectPoolBase in ObjectPoolList)
			{
				objectPoolBase.ShutDown();
			}
			
			ObjectPoolList.Clear();
		}

		private static bool TryGetObjectPool<T>(string poolName, out ObjectPoolBase objectPool) where T : ObjectBase
		{
			objectPool = null;
			foreach (var objPool in ObjectPoolList)
			{
				if (objPool.ObjectType == typeof(T) && objPool.Name.Equals(poolName))
				{
					objectPool = objPool;
					return true;
				}
			}

			return false;
		}
	}
}