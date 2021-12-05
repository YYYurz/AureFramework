//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace AureFramework.ObjectPool {
	public class O : AureObjectBase {
		public void Function() {
				
		}
	}
	
	public sealed partial class ObjectPoolModule : AureFrameworkModule, IObjectPoolModule {
		private class ObjectPool<T> : IObjectPool where T : AureObjectBase {
			private List<T> unusedObjectList = new List<T>();
			private List<T> usingObjectList = new List<T>();
			private int usingCount;
			private int unusedCount;
			private int capacity;
			private float expireTime;

			public ObjectPool() {
				
			}

			public int UsingCount
			{
				get
				{
					return usingObjectList.Count;
				}
			}

			public int UnusedCount
			{
				get
				{
					return unusedObjectList.Count;
				}
			}

			public int Capacity
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

			public float ExpireTime
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

			public void Update(float elapseTime, float realElapseTime) {
				
			}

			public void ShutDown() {
				
			}
			
			public AureObjectBase Spawn() {
				return null;
			}

			public AureObjectBase Spawn(string name) {
				return null;
			}

			public void Recycle(AureObjectBase obj) {
				
			}

			public void Recycle(string name, AureObjectBase obj) {
				
			}

			public void SetLock(string name) {
				
			}

			public void SetUnlock(string name) {
				
			}

			public void LockAll() {
				
			}

			public void UnlockAll() {
				
			}

			public void ReleaseAll() {
				
			}

			public void ReleaseAllUnused() {
				
			}

			private void InternalReleaseObject() {
				if (UnusedCount <= 0) {
					return;
				}
				
				
			}
		}
	}
}