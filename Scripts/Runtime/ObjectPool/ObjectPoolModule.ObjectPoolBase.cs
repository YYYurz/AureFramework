//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;

namespace AureFramework.ObjectPool
{
	public sealed partial class ObjectPoolModule : AureFrameworkModule, IObjectPoolModule
	{
		/// <summary>
		/// 内部对象池基类
		/// </summary>
		private abstract class ObjectPoolBase
		{
			/// <summary>
			/// 获取或设置对象池名称
			/// </summary>
			public abstract string Name
			{
				get;
				set;
			}

			/// <summary>
			/// 获取对象类型
			/// </summary>
			public abstract Type ObjectType
			{
				get;
			}

			/// <summary>
			/// 获取或设置对象池容量
			/// </summary>
			public abstract int Capacity
			{
				get;
				set;
			}

			/// <summary>
			/// 获取或设置对象池过期秒数
			/// </summary>
			public abstract float ExpireTime
			{
				get;
				set;
			}

			/// <summary>
			/// 获取对象池信息
			/// </summary>
			/// <returns></returns>
			public abstract ObjectPoolInfo GetObjectPoolInfo();

			/// <summary>
			/// 轮询
			/// </summary>
			/// <param name="elapseTime"> 距离上一帧的流逝时间，秒单位 </param>
			/// <param name="realElapseTime"> 距离上一帧的真实流逝时间，秒单位 </param>
			public abstract void Update(float elapseTime, float realElapseTime);

			/// <summary>
			/// 销毁
			/// </summary>
			public abstract void ShutDown();

			/// <summary>
			/// 释放所有没有使用中的对象
			/// </summary>
			public abstract void ReleaseAllUnused();
		}
	}
}