//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace AureFramework.ObjectPool {
	/// <summary>
	/// 对象池模块
	/// </summary>
	public sealed partial class ObjectPoolModule : AureFrameworkModule, IObjectPoolModule {
		private readonly List<ObjectPoolBase> objectPoolList = new List<ObjectPoolBase>(); 

		/// <summary>
		/// 模块优先级，最小的优先轮询
		/// </summary>
		public override int Priority => 4;

		/// <summary>
		/// 获取对象池数量
		/// </summary>
		public int Count
		{
			get
			{
				return objectPoolList.Count;
			}
		}

		/// <summary>
		/// 模块初始化，只在第一次被获取时调用一次
		/// </summary>
		public override void Init() {
			
		}

		/// <summary>
		/// 框架轮询
		/// </summary>
		/// <param name="elapseTime"> 距离上一帧的流逝时间，秒单位 </param>
		/// <param name="realElapseTime"> 距离上一帧的真实流逝时间，秒单位 </param>
		public override void Tick(float elapseTime, float realElapseTime) {
			foreach (var objectPool in objectPoolList) {
				objectPool.Update(elapseTime, realElapseTime);
			}
		}

		/// <summary>
		/// 框架清理
		/// </summary>
		public override void Clear() {
			InternalDestroyAllObjectPool();
		}

		/// <summary>
		/// 获取对象池
		/// </summary>
		/// <typeparam name="T"> 对象类型 </typeparam>
		/// <returns></returns>
		public IObjectPool<T> GetObjectPool<T>() where T : AureObjectBase {
			TryGetObjectPool<T>(out var objectPool);
			return (IObjectPool<T>) objectPool;
		}

		/// <summary>
		/// 创建对象池
		/// </summary>
		/// <param name="capacity"> 容量 </param>
		/// <param name="expireTime"> 自动释放时间 </param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public IObjectPool<T> CreateObjectPool<T>(int capacity, float expireTime) where T : AureObjectBase {
			return InternalCreateObjectPool<T>(capacity, expireTime);
		}

		/// <summary>
		/// 销毁对象池
		/// </summary>
		public void DestroyObjectPool<T>(IObjectPool<T> objPool) where T : AureObjectBase {
			InternalDestroyObjectPool<T>(objPool);
		}
		
		/// <summary>
		/// 释放所有对象池中未使用的对象
		/// </summary>
		public void ReleaseAllUnused() {
			foreach (var objectPool in objectPoolList) {
				objectPool.ReleaseAllUnused();
			}
		}

		private IObjectPool<T> InternalCreateObjectPool<T>(int capacity, float expireTime) where T : AureObjectBase {
			if (TryGetObjectPool<T>(out var objPool)) {
				Debug.LogError($"AureFramework ObjectPoolModule : Object Pool is already exists, object type : {objPool.ObjectType.FullName}.");
				return null;
			}
			
			var objectPool = new ObjectPool<T>(capacity, expireTime);
			objectPoolList.Add(objectPool);
			return objectPool;
		}

		private void InternalDestroyObjectPool<T>(IObjectPool<T> objPool) where T : AureObjectBase {
			if (objPool == null || !TryGetObjectPool<T>(out var objectPool)) {
				Debug.LogError($"AureFramework ObjectPoolModule : Object Pool is invalid.");
				return;
			}
			
			objectPool.ShutDown();
			objectPoolList.Remove(objectPool);
		}
		
		private void InternalDestroyAllObjectPool() {
			for (var i = 0; i < Count; i++) {
				var objectPool = objectPoolList[i];
				objectPool.ShutDown();
				objectPoolList.Remove(objectPool);
			}
		}

		private bool TryGetObjectPool<T>(out ObjectPoolBase objectPool) where T : AureObjectBase {
			objectPool = null;
			foreach (var objPool in objectPoolList) {
				if (objPool.ObjectType == typeof(T)) {
					objectPool = objPool;
					return true;
				}
			}

			return false;
		}
	}
}