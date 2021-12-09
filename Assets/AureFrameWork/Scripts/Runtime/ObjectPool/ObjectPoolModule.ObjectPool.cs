//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using AureFramework.ReferencePool;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace AureFramework.ObjectPool {
	public sealed partial class ObjectPoolModule : AureFrameworkModule, IObjectPoolModule {
		/// <summary>
		/// 内部对象池
		/// </summary>
		/// <typeparam name="T"></typeparam>
		private class ObjectPool<T> : ObjectPoolBase, IObjectPool<T> where T : UnityObject {
			private readonly List<ObjectBase> objectList = new List<ObjectBase>();
			private readonly IReferencePoolModule referencePoolModule;
			private string name;
			private int capacity;
			private float expireTime;
			private float autoReleaseTime;
			private readonly float autoReleaseInterval;

			public ObjectPool(string name, int capacity, float expireTime) {
				referencePoolModule = Aure.GetModule<IReferencePoolModule>();
				autoReleaseInterval = 1f;
				autoReleaseTime = 0f;
				this.name = name;
				this.capacity = capacity;
				this.expireTime = expireTime;
			}
			
			/// <summary>
			/// 获取或设置对象池名称
			/// </summary>
			public override string Name
			{
				get
				{
					return name;
				}
				set
				{
					name = value;
				}
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
					var num = 0;
					foreach (var internalObject in objectList) {
						num += internalObject.IsInUse ? 1 : 0;
					}
					return num;
				}
			}

			/// <summary>
			/// 获取未使用对象的数量
			/// </summary>
			public int UnusedCount
			{
				get
				{
					var num = 0;
					foreach (var internalObject in objectList) {
						num += internalObject.IsInUse ? 0 : 1;
					}
					return num;
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
			/// 获取对象池信息
			/// </summary>
			/// <returns></returns>
			public override ObjectPoolInfo GetObjectPoolInfo() {
				var objectInfos = new ObjectInfo[objectList.Count];
				for (var i = 0; i < objectList.Count; i++) {
					objectInfos[i] = objectList[i].GetObjectInfo();
				}

				return new ObjectPoolInfo(ObjectType, name, capacity, UsingCount, UnusedCount, expireTime, objectInfos);
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
				objectList.Clear();
			}

			/// <summary>
			/// 注册一个新创建的对象
			/// </summary>
			/// <param name="obj"> 对象 </param>
			/// <param name="isNeed"> 是否需要注册后的对象 </param>
			/// <param name="objName"> 对象名称 </param>
			/// <returns></returns>
			public IObject<T> Register(T obj, bool isNeed, string objName = null) {
				if (obj == null) {
					Debug.LogError("AureFramework ObjectPoolModule : Object is null.");
					return null;
				}
				
				var newObject = InternalCreateObject(obj, isNeed, objName);
				return (IObject<T>)newObject;
			}

			/// <summary>
			/// 获取对象池中任意一个对象
			/// </summary>
			/// <returns></returns>
			public IObject<T> Spawn() {
				return (IObject<T>)InternalSpawn();
			}

			/// <summary>
			/// 获取对象
			/// </summary>
			/// <param name="objName"> 对象名称 </param>
			/// <returns></returns>
			public IObject<T> Spawn(string objName) {
				return (IObject<T>)InternalSpawn(objName);
			}

			/// <summary>
			/// 回收对象
			/// </summary>
			/// <param name="obj"> 对象 </param>
			public void Recycle(IObject<T> obj) {
				if (obj == null) {
					Debug.LogError("AureFramework ObjectPoolModule : Object is null.");
					return;
				}
				
				InternalRecycle((ObjectBase)obj);
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

			private ObjectBase InternalCreateObject(T obj, bool isNeed, string objName = null) {
				for (var i = 0; i < objectList.Count; i++) {
					if (objectList[i].TargetId.Equals(obj.GetHashCode())) {
						Debug.LogError("AureFramework ObjectPoolModule : Register failed because the object is already exists.");
						return InternalSpawn(objectList[i].Name);
					}
				}
				
				var internalObj = Object<T>.Create(obj, objName ?? string.Empty);
				internalObj.IsInUse = isNeed;
				objectList.Add(internalObj);

				if (isNeed) {
					internalObj.OnSpawn();
					internalObj.LastUseTime = DateTime.UtcNow;
					return internalObj;
				}

				return null;
			}
			
			private ObjectBase InternalSpawn(string objName = null) {
				if (UnusedCount == 0) {
					return null;
				}
				
				var internalObjectName = objName ?? string.Empty;
				ObjectBase obj = null;
				
				foreach (var internalObj in objectList) {
					if (internalObj.Name.Equals(internalObjectName) && !internalObj.IsInUse) {
						obj = internalObj;
						break;
					}
				}

				if (obj != null) {
					obj.OnSpawn();
					obj.LastUseTime = DateTime.UtcNow;
					obj.IsInUse = true;
				}

				return obj;
			}

			private void InternalRecycle(ObjectBase internalObject) {
				if (!objectList.Contains(internalObject)) {
					return;
				}

				if (UsingCount + UnusedCount >= capacity) {
					InternalReleaseObject(internalObject);
					return;
				}
				
				internalObject.LastUseTime = DateTime.UtcNow;
				internalObject.IsInUse = false;
				internalObject.OnRecycle();
			}

			private void InternalSetLockAll(bool isLock) {
				foreach (var internalObject in objectList) {
					internalObject.IsLock = isLock;
				}
			}
			
			private void InternalReleaseUnusedObject(bool isCheckTime) {
				autoReleaseTime = 0f;
				if (UnusedCount <= 0) {
					return;
				}

				var dateNow = DateTime.UtcNow;
				for (var i = 0; i < UnusedCount; i++) {
					var obj = objectList[i];
					var lastUseTime = obj.LastUseTime.AddSeconds(-ExpireTime);
					if (!isCheckTime || DateTime.Compare(dateNow, lastUseTime) <= 0) {
						InternalReleaseObject(obj);
					}
				}
			}

			private void InternalReleaseObject(ObjectBase internalObject) {
				if (internalObject == null || internalObject.IsLock) {
					return;
				}
				
				internalObject.OnRelease();
				referencePoolModule.Release(internalObject);
				objectList.Remove(internalObject);
			}
		}
	}
}