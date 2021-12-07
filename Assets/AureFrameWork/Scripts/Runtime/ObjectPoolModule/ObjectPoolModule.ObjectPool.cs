//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using AureFramework.ReferencePool;
using UnityEngine;

namespace AureFramework.ObjectPool {
	public sealed partial class ObjectPoolModule : AureFrameworkModule, IObjectPoolModule {
		/// <summary>
		/// 内部对象池
		/// </summary>
		/// <typeparam name="T"></typeparam>
		private class ObjectPool<T> : ObjectPoolBase, IObjectPool<T> where T : AureObjectBase {
			private readonly List<T> unusedObjectList = new List<T>();
			private readonly List<T> usingObjectList = new List<T>();
			private readonly IReferencePoolModule referencePoolModule;
			private int capacity;
			private float expireTime;
			private float autoReleaseTime;
			private readonly float autoReleaseInterval;

			public ObjectPool(int capacity, float expireTime) {
				referencePoolModule = Aure.GetModule<IReferencePoolModule>();
				autoReleaseInterval = 1f;
				autoReleaseTime = 0f;
				this.capacity = capacity;
				this.expireTime = expireTime;
			}

			/// <summary>
			/// 获取对象类型
			/// </summary>
			public override Type ObjectType
			{
				get
				{
					return typeof(T);
				}
			}

			/// <summary>
			/// 获取使用中对象的数量
			/// </summary>
			public int UsingCount
			{
				get
				{
					return usingObjectList.Count;
				}
			}

			/// <summary>
			/// 获取未使用对象的数量
			/// </summary>
			public int UnusedCount
			{
				get
				{
					return unusedObjectList.Count;
				}
			}

			/// <summary>
			/// 获取或设置对象池容量
			/// </summary>
			public override int Capacity
			{
				get
				{
					return capacity;
				}
				set
				{
					if (value < 0) {
						Debug.LogError("AureFramework ObjectPoolModule : The capacity of the object pool cannot be less than zero.");
						return;
					}

					if (capacity.Equals(value)) {
						return;
					}
					
					capacity = value;
				}
			}

			/// <summary>
			/// 获取或设置对象池过期秒数
			/// </summary>
			public override float ExpireTime
			{
				get
				{
					return expireTime;
				}
				set
				{
					if (value < 0) {
						Debug.LogError("AureFramework ObjectPoolModule : The capacity of the object pool cannot be less than zero.");
						return;
					}

					if (expireTime.Equals(value)) {
						return;
					}
					
					expireTime = value;
				}
			}

			/// <summary>
			/// 轮询
			/// </summary>
			/// <param name="elapseTime"> 距离上一帧的流逝时间，秒单位 </param>
			/// <param name="realElapseTime"> 距离上一帧的真实流逝时间，秒单位 </param>
			public override void Update(float elapseTime, float realElapseTime) {
				autoReleaseTime += realElapseTime;
				if (autoReleaseTime < autoReleaseInterval) {
					return;
				}

				InternalReleaseUnusedObject(true);
			}

			/// <summary>
			/// 销毁对象池，清除对正在使用中对象的引用，释放所有闲置的对象
			/// </summary>
			public override void ShutDown() {
				ReleaseAllUnused();
				usingObjectList.Clear();
			}

			/// <summary>
			/// 注册一个新创建的对象
			/// </summary>
			/// <param name="obj"> 对象 </param>
			public void Register(T obj) {
				if (unusedObjectList.Contains(obj) || usingObjectList.Contains(obj)) {
					return;
				}
				unusedObjectList.Add(obj);
			}

			/// <summary>
			/// 获取对象池中任意一个对象
			/// </summary>
			/// <returns></returns>
			public T Spawn() {
				return InternalSpawn();
			}

			/// <summary>
			/// 获取对象
			/// </summary>
			/// <param name="name"> 对象名称 </param>
			/// <returns></returns>
			public T Spawn(string name) {
				return InternalSpawn(name);
			}

			/// <summary>
			/// 回收对象
			/// </summary>
			/// <param name="obj"> 对象 </param>
			public void Recycle(T obj) {
				InternalRecycle(obj);
			}

			/// <summary>
			/// 所有对象加锁
			/// </summary>
			public void LockAll() {
				InternalSetLockAll(true);
			}

			/// <summary>
			/// 所有对象解锁
			/// </summary>
			public void UnlockAll() {
				InternalSetLockAll(false);
			}

			/// <summary>
			/// 释放所有没有使用中的对象
			/// </summary>
			public override void ReleaseAllUnused() {
				InternalReleaseUnusedObject(false);
			}

			private void InternalRegister(T obj) {
				
			}
			
			private T InternalSpawn(string name = null) {
				if (UnusedCount == 0) {
					return null;
				}
				
				var objName = name ?? string.Empty;
				T obj = null;
				
				foreach (var unusedObj in unusedObjectList) {
					if (unusedObj.Name.Equals(objName)) {
						obj = unusedObj;
						break;
					}
				}

				if (obj != null) {
					obj.OnSpawn();
					obj.LastUseTime = DateTime.UtcNow;
					unusedObjectList.Remove(obj);
					usingObjectList.Add(obj);
				}

				return obj;
			}

			private void InternalRecycle(T obj) {
				if (obj == null) {
					return;
				}

				if (!usingObjectList.Contains(obj)) {
					return;
				}

				if (UsingCount + UnusedCount >= capacity) {
					InternalReleaseObject(obj);
					return;
				}
				
				obj.LastUseTime = DateTime.UtcNow;
				obj.OnRecycle();
				unusedObjectList.Add(obj);
				usingObjectList.Remove(obj);
			}

			private void InternalSetLockAll(bool isLock) {
				foreach (var unusedObject in unusedObjectList) {
					unusedObject.Locked = isLock;
				}
				
				foreach (var usingObject in usingObjectList) {
					usingObject.Locked = isLock;
				}
			}
			
			private void InternalReleaseUnusedObject(bool isCheckTime) {
				autoReleaseTime = 0f;
				if (UnusedCount <= 0) {
					return;
				}

				var dateNow = DateTime.UtcNow;
				for (var i = 0; i < UnusedCount; i++) {
					var obj = unusedObjectList[i];
					var lastUseTime = obj.LastUseTime.AddSeconds(-ExpireTime);
					if (!isCheckTime || DateTime.Compare(dateNow, lastUseTime) <= 0) {
						InternalReleaseObject(obj);
					}
				}
			}

			private void InternalReleaseObject(T obj) {
				if (obj == null || obj.Locked) {
					return;
				}
				
				obj.OnRelease();
				referencePoolModule.Release(obj);
				unusedObjectList.Remove(obj);
				usingObjectList.Remove(obj);
			}
		}
	}
}