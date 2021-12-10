//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using AureFramework.ReferencePool;
using Object = UnityEngine.Object;

namespace AureFramework.ObjectPool {
	public sealed partial class ObjectPoolModule : AureFrameworkModule, IObjectPoolModule {
		/// <summary>
		/// 内部对象
		/// </summary>
		private sealed class Object<T> : ObjectBase, IObject<T> where T : Object {
			private T target;
			private DateTime lastUseTime;
			private string name;
			private bool isLock;
			private bool isInUse;

			private Action onSpawnCallBack;
			private Action onRecycleCallBack;
			private Action onReleaseCallBack;

			/// <summary>
			/// 获取对象唯一Id
			/// </summary>
			public override int TargetId
			{
				get
				{
					return target.GetHashCode();
				}
			}

			/// <summary>
			/// 获取对象
			/// </summary>
			public T Target
			{
				get
				{
					return target;
				}
			}

			/// <summary>
			/// 获取或设置对象上次使用时间。
			/// </summary>
			public override DateTime LastUseTime
			{
				get
				{
					return lastUseTime;
				}
				set
				{
					lastUseTime = value;
				}
			}

			/// <summary>
			/// 获取对象名称
			/// </summary>
			public override string Name
			{
				get
				{
					return name;
				}
			}

			/// <summary>
			/// 获取或设置对象是否加锁
			/// </summary>
			public override bool IsLock
			{
				get
				{
					return isLock;
				}
				set
				{
					isLock = value;
				}
			}

			/// <summary>
			/// 设置或获取对象是否正在被使用
			/// </summary>
			public override bool IsInUse
			{
				get
				{
					return isInUse;
				}
				set
				{
					isInUse = value;
				}
			}

			/// <summary>
			/// 获取对象信息
			/// </summary>
			/// <returns></returns>
			public override ObjectInfo GetObjectInfo() {
				return new ObjectInfo(name, lastUseTime, isLock, isInUse);
			}

			/// <summary>
			/// 引用池创建对象
			/// </summary>
			/// <param name="target"> 对象 </param>
			/// <param name="name"> 对象名称 </param>
			/// <returns></returns>
			public static Object<T> Create( T target, string name) {
				var internalObject = Aure.GetModule<IReferencePoolModule>().Acquire<Object<T>>();
				internalObject.target = target;
				internalObject.name = name;

				return internalObject;
			}

			/// <summary>
			/// 清理
			/// </summary>
			public override void Clear() {
				target = null;
			}
		}
	}
}