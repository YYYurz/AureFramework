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

namespace AureFramework.ObjectPool
{
	public sealed partial class ObjectPoolModule : AureFrameworkModule, IObjectPoolModule
	{
		/// <summary>
		/// 内部对象池
		/// </summary>
		/// <typeparam name="T"></typeparam>
		private class ObjectPool<T> : ObjectPoolBase, IObjectPool<T> where T : ObjectBase
		{
			private readonly List<Object<T>> objectList = new List<Object<T>>();
			private readonly IReferencePoolModule referencePoolModule;
			private string name;
			private int capacity;
			private float expireTime;
			private float autoReleaseTime;
			private readonly float autoReleaseInterval;

			public ObjectPool(string name, int capacity, float expireTime)
			{
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
					foreach (var internalObject in objectList)
					{
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
					foreach (var internalObject in objectList)
					{
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
					if (value < 0)
					{
						Debug.LogError("ObjectPoolModule : The capacity of the object pool cannot be less than zero.");
						return;
					}

					if (capacity.Equals(value))
					{
						return;
					}

					capacity = value;
					InternalTryReleaseUnusedObject(false);
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
					if (value < 0)
					{
						Debug.LogError("ObjectPoolModule : The expire time of the object pool cannot be less than zero.");
						return;
					}

					if (expireTime.Equals(value))
					{
						return;
					}

					expireTime = value;
				}
			}

			/// <summary>
			/// 获取对象池信息
			/// </summary>
			/// <returns></returns>
			public override ObjectPoolInfo GetObjectPoolInfo()
			{
				var objectInfos = new ObjectInfo[objectList.Count];
				for (var i = 0; i < objectList.Count; i++)
				{
					objectInfos[i] = objectList[i].GetObjectInfo();
				}

				return new ObjectPoolInfo(ObjectType, name, capacity, UsingCount, UnusedCount, expireTime, objectInfos);
			}

			/// <summary>
			/// 轮询
			/// </summary>
			/// <param name="elapseTime"> 距离上一帧的流逝时间，秒单位 </param>
			/// <param name="realElapseTime"> 距离上一帧的真实流逝时间，秒单位 </param>
			public override void Update(float elapseTime, float realElapseTime)
			{
				autoReleaseTime += realElapseTime;
				if (autoReleaseTime < autoReleaseInterval)
				{
					return;
				}

				InternalTryReleaseUnusedObject(true);
			}

			/// <summary>
			/// 销毁对象池，清除对正在使用中对象的引用，释放所有闲置的对象
			/// </summary>
			public override void ShutDown()
			{
				ReleaseAllUnused();
				objectList.Clear();
			}

			/// <summary>
			/// 注册一个新创建的对象
			/// </summary>
			/// <param name="obj"> 对象 </param>
			/// <param name="isNeed"> 是否需要使用注册后的对象 </param>
			/// <param name="objName"> 对象名称 </param>
			/// <returns></returns>
			public void Register(T obj, bool isNeed, string objName = null)
			{
				if (obj == null)
				{
					Debug.LogError("ObjectPoolModule : Object is null.");
					return;
				}

				if (objectList.Count >= capacity)
				{
					Debug.LogWarning($"ObjectPoolModule : Register failed because capacity exceeded, Object Name :{objName}");
					return;
				}

				InternalTryCreateObject(obj, isNeed, objName);
			}

			/// <summary>
			/// 是否能获取对象
			/// </summary>
			/// <param name="objName"> 对象名称 </param>
			/// <returns></returns>
			public bool CanSpawn(string objName)
			{
				foreach (var internalObject in objectList)
				{
					if (internalObject.Name.Equals(objName) && !internalObject.IsInUse)
					{
						return true;
					}
				}

				return false;
			}

			/// <summary>
			/// 获取对象池中任意一个对象
			/// </summary>
			/// <returns></returns>
			public T Spawn()
			{
				return (T) InternalTrySpawn();
			}

			/// <summary>
			/// 获取对象
			/// </summary>
			/// <param name="objName"> 对象名称 </param>
			/// <returns></returns>
			public T Spawn(string objName)
			{
				return (T) InternalTrySpawn(objName);
			}

			/// <summary>
			/// 回收对象
			/// </summary>
			/// <param name="obj"> 对象 </param>
			public void Recycle(T obj)
			{
				if (obj == null)
				{
					Debug.LogError("ObjectPoolModule : Object is null.");
					return;
				}

				InternalTryRecycle(obj);
			}

			/// <summary>
			/// 所有对象加锁
			/// </summary>
			public void LockAll()
			{
				foreach (var internalObject in objectList)
				{
					internalObject.IsLock = true;
				}
			}

			/// <summary>
			/// 所有对象解锁
			/// </summary>
			public void UnlockAll()
			{
				foreach (var internalObject in objectList)
				{
					internalObject.IsLock = false;
				}
			}

			/// <summary>
			/// 对象加锁（同名所有对象）
			/// </summary>
			/// <param name="objName"> 对象名称 </param>
			public void Lock(string objName)
			{
				foreach (var internalObject in objectList)
				{
					if (internalObject.Name.Equals(objName))
					{
						internalObject.IsLock = true;
					}
				}
			}

			/// <summary>
			/// 对象解锁（同名所有对象）
			/// </summary>
			/// <param name="objName"> 对象名称 </param>
			public void Unlock(string objName)
			{
				foreach (var internalObject in objectList)
				{
					if (internalObject.Name.Equals(objName))
					{
						internalObject.IsLock = false;
					}
				}
			}

			/// <summary>
			/// 释放所有没有使用中的对象
			/// </summary>
			public override void ReleaseAllUnused()
			{
				InternalTryReleaseUnusedObject(false);
			}

			/// <summary>
			/// 对象是否存在
			/// </summary>
			/// <param name="objName"> 对象名称 </param>
			/// <returns></returns>
			public bool IsHasObject(string objName)
			{
				foreach (var internalObject in objectList)
				{
					if (internalObject.Name.Equals(objName))
					{
						return true;
					}
				}

				return false;
			}

			private void InternalTryCreateObject(T obj, bool isNeed, string objName = null)
			{
				foreach (var internalObject in objectList)
				{
					if (internalObject.ExternalObject.Equals(obj))
					{
						Debug.LogError("ObjectPoolModule : Register failed because the object is already exists.");
						return;
					}
				}

				var internalObj = Object<T>.Create(obj, objName ?? string.Empty, isNeed);
				objectList.Add(internalObj);
			}

			private ObjectBase InternalTrySpawn(string objName = null)
			{
				if (UnusedCount == 0)
				{
					return null;
				}

				var internalObjectName = objName ?? string.Empty;
				foreach (var internalObject in objectList)
				{
					if (internalObject.Name.Equals(internalObjectName) && !internalObject.IsInUse)
					{
						return internalObject.Spawn();
					}
				}

				return null;
			}

			private void InternalTryRecycle(ObjectBase objectBase)
			{
				foreach (var internalObject in objectList)
				{
					if (internalObject.ExternalObject.Equals(objectBase))
					{
						internalObject.Recycle();
						return;
					}
				}

				Debug.LogError("ObjectPoolModule : Can not Recycle because it was not generated from the object pool.");
			}


			private void InternalTryReleaseUnusedObject(bool isCheckTime)
			{
				autoReleaseTime = 0f;
				if (UnusedCount <= 0)
				{
					return;
				}

				var dateNow = DateTime.UtcNow;
				for (var i = objectList.Count - 1; i >= 0; i--)
				{
					var internalObject = objectList[i];
					if (isCheckTime)
					{
						if (internalObject.Release(dateNow, expireTime))
						{
							referencePoolModule.Release(internalObject);
							objectList.Remove(internalObject);
						}
					}
					else
					{
						if (internalObject.ReleaseIgnoreTime())
						{
							referencePoolModule.Release(internalObject);
							objectList.Remove(internalObject);
						}
					}
				}
			}
		}
	}
}