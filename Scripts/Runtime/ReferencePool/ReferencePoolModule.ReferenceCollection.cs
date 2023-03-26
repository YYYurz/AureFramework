//------------------------------------------------------------
// AureFramework
// Developed By YYYurz
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace AureFramework.ReferencePool
{
	public sealed partial class ReferencePoolModule : AureFrameworkModule, IReferencePoolModule
	{
		/// <summary>
		/// 引用容器
		/// </summary>
		private sealed class ReferenceCollection
		{
			private readonly Type referenceType;
			private readonly Queue<IReference> referenceQue;
			private int usingReferenceCount;
			private int acquireReferenceCount;
			private int releaseReferenceCount;

			public ReferenceCollection(Type referenceType)
			{
				this.referenceType = referenceType;
				referenceQue = new Queue<IReference>();
				usingReferenceCount = 0;
				acquireReferenceCount = 0;
				releaseReferenceCount = 0;
			}

			public int UnusedReferenceCount
			{
				get
				{
					return referenceQue.Count;
				}
			}

			public int UsingReferenceCount
			{
				get
				{
					return usingReferenceCount;
				}
			}

			public int AcquireReferenceCount
			{
				get
				{
					return acquireReferenceCount;
				}
			}

			public int ReleaseReferenceCount
			{
				get
				{
					return releaseReferenceCount;
				}
			}

			public IReference Acquire()
			{
				usingReferenceCount++;
				acquireReferenceCount++;
				lock (referenceQue)
				{
					if (referenceQue.Count > 0)
					{
						return referenceQue.Dequeue();
					}
				}

				return (IReference) Activator.CreateInstance(referenceType);
			}

			public void Release(IReference reference)
			{
				reference.Clear();
				lock (referenceQue)
				{
					referenceQue.Enqueue(reference);
				}

				releaseReferenceCount++;
				usingReferenceCount--;
			}

			public void Add(int num)
			{
				lock (referenceQue)
				{
					while (num-- > 0)
					{
						referenceQue.Enqueue((IReference) Activator.CreateInstance(referenceType));
					}
				}
			}

			public void Remove(int num)
			{
				lock (referenceQue)
				{
					if (num > referenceQue.Count)
					{
						num = referenceQue.Count;
					}

					while (num-- > 0 && referenceQue.Count > 0)
					{
						referenceQue.Dequeue();
					}
				}
			}

			public void RemoveAll()
			{
				lock (referenceQue)
				{
					referenceQue.Clear();
				}
			}
		}
	}
}