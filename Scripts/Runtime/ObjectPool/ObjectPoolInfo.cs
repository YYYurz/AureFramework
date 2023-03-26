//------------------------------------------------------------
// AureFramework
// Developed By YYYurz
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;

namespace AureFramework.ObjectPool
{
	/// <summary>
	/// 对象池信息
	/// </summary>
	public sealed class ObjectPoolInfo
	{
		private readonly Type objectType;
		private readonly string name;
		private readonly int capacity;
		private readonly int usingCount;
		private readonly int unusedCount;
		private readonly float expireTime;
		private readonly ObjectInfo[] objectInfoList;

		public ObjectPoolInfo(Type objectType, string name, int capacity, int usingCount, int unusedCount, float expireTime, ObjectInfo[] objectInfoList)
		{
			this.objectType = objectType;
			this.name = name;
			this.capacity = capacity;
			this.usingCount = usingCount;
			this.unusedCount = unusedCount;
			this.expireTime = expireTime;
			this.objectInfoList = objectInfoList;
		}

		/// <summary>
		/// 获取对象类型
		/// </summary>
		public Type ObjectType
		{
			get
			{
				return objectType;
			}
		}

		/// <summary>
		/// 获取对象池名称
		/// </summary>
		public string Name
		{
			get
			{
				return name;
			}
		}

		/// <summary>
		/// 获取对象池容量
		/// </summary>
		public int Capacity
		{
			get
			{
				return capacity;
			}
		}

		/// <summary>
		/// 获取使用中对象的数量
		/// </summary>
		public int UsingCount
		{
			get
			{
				return usingCount;
			}
		}

		/// <summary>
		/// 获取未使用对象的数量
		/// </summary>
		public int UnusedCount
		{
			get
			{
				return unusedCount;
			}
		}

		/// <summary>
		/// 获取对象池过期秒数
		/// </summary>
		public float ExpireTime
		{
			get
			{
				return expireTime;
			}
		}

		/// <summary>
		/// 获取所有对象信息
		/// </summary>
		public ObjectInfo[] ObjectInfoList
		{
			get
			{
				return objectInfoList;
			}
		}
	}
}