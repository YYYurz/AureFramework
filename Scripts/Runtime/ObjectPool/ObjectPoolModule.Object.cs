//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using AureFramework.ReferencePool;

namespace AureFramework.ObjectPool
{
	public sealed partial class ObjectPoolModule : AureFrameworkModule, IObjectPoolModule
	{
		/// <summary>
		/// 内部对象
		/// </summary>
		private sealed class Object<T> : IReference where T : ObjectBase
		{
			private T externalObject;
			private DateTime lastUseTime;
			private string name;
			private bool isInUse;

			/// <summary>
			/// 获取对象
			/// </summary>
			public T ExternalObject
			{
				get
				{
					return externalObject;
				}
			}

			/// <summary>
			/// 获取对象名称
			/// </summary>
			public string Name
			{
				get
				{
					return name;
				}
			}

			/// <summary>
			/// 获取或设置对象是否加锁
			/// </summary>
			public bool IsLock
			{
				get
				{
					return ExternalObject.IsLock;
				}
				set
				{
					ExternalObject.IsLock = value;
				}
			}

			/// <summary>
			/// 设置或获取对象是否正在被使用
			/// </summary>
			public bool IsInUse
			{
				get
				{
					return isInUse;
				}
			}

			/// <summary>
			/// 获取对象信息
			/// </summary>
			/// <returns></returns>
			public ObjectInfo GetObjectInfo()
			{
				return new ObjectInfo(name, lastUseTime, IsLock, isInUse);
			}

			/// <summary>
			/// 获取对象
			/// </summary>
			/// <returns></returns>
			public T Spawn()
			{
				isInUse = true;
				lastUseTime = DateTime.UtcNow;
				externalObject.OnSpawn();
				return externalObject;
			}

			/// <summary>
			/// 回收对象
			/// </summary>
			public void Recycle()
			{
				isInUse = false;
				lastUseTime = DateTime.UtcNow;
				externalObject.OnRecycle();
			}

			/// <summary>
			/// 释放对象
			/// </summary>
			/// <param name="dateNow"> 当前世界协调时间 </param>
			/// <param name="expireTime"> 过期秒数 </param>
			public bool Release(DateTime dateNow, float expireTime)
			{
				var tempTime = dateNow.AddSeconds(-expireTime);
				if (DateTime.Compare(tempTime, lastUseTime) > 0 && !ExternalObject.IsLock && !isInUse)
				{
					externalObject.OnRelease();
					return true;
				}

				return false;
			}

			/// <summary>
			/// 忽略时间释放对象
			/// </summary>
			/// <returns></returns>
			public bool ReleaseIgnoreTime()
			{
				if (!ExternalObject.IsLock && !isInUse)
				{
					externalObject.OnRelease();
					return true;
				}

				return false;
			}

			/// <summary>
			/// 引用池创建对象
			/// </summary>
			/// <param name="obj"> 对象 </param>
			/// <param name="name"> 对象名称 </param>
			/// <param name="isNeed"> 是否需要使用刚生成的对象 </param>
			/// <returns></returns>
			public static Object<T> Create(T obj, string name, bool isNeed)
			{
				var internalObject = Aure.GetModule<IReferencePoolModule>().Acquire<Object<T>>();
				internalObject.externalObject = obj;
				internalObject.lastUseTime = DateTime.UtcNow;
				internalObject.name = name;
				internalObject.isInUse = isNeed;

				return internalObject;
			}

			public void Clear()
			{
				Aure.GetModule<IReferencePoolModule>().Release(externalObject);
				externalObject = null;
				lastUseTime = default;
				name = null;
				isInUse = false;
			}
		}
	}
}