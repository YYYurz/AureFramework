//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using AureFramework.ReferencePool;
using Object = UnityEngine.Object;

namespace AureFramework.ObjectPool {
	public abstract class AureObjectBase : IReference {
		private Object target;
		private DateTime lastUseTime;
		private string name;
		private bool locked;
		private bool isInUse;
		
		/// <summary>
		/// 获取对象
		/// </summary>
		public Object Target
		{
			get
			{
				return target;
			}
		}
		
		/// <summary>
		/// 获取或设置对象上次使用时间。
		/// </summary>
		public DateTime LastUseTime
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
		public bool Locked
		{
			get
			{
				return locked;
			}
			set
			{
				locked = value;
			}
		}
		
		

		/// <summary>
		/// 初始化对象
		/// </summary>
		/// <param name="targetObj"> 对象 </param>
		protected void Init(Object targetObj) {
			Init(null, targetObj);
		}

		/// <summary>
		/// 初始化对象
		/// </summary>
		/// <param name="objName"> 对象名称 </param>
		/// <param name="targetObj"> 对象 </param>
		protected void Init(string objName, Object targetObj) {
			name = objName ?? string.Empty;
			target = targetObj;
		}

		/// <summary>
		/// 获取对象时触发
		/// </summary>
		public virtual void OnSpawn() {
			
		}

		/// <summary>
		/// 回收对象时触发
		/// </summary>
		public virtual void OnRecycle() {
			
		}

		/// <summary>
		/// 释放对象时触发
		/// </summary>
		public virtual void OnRelease() {
			
		}
		
		public void Clear() {
			target = null;
		}
	}
}