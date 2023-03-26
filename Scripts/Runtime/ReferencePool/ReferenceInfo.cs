//------------------------------------------------------------
// AureFramework
// Developed By YYYurz
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

namespace AureFramework.ReferencePool
{
	/// <summary>
	/// 引用信息
	/// </summary>
	public readonly struct ReferenceInfo
	{
		private readonly string typeName;
		private readonly int unusedReferenceCount;
		private readonly int usingReferenceCount;
		private readonly int acquireReferenceCount;
		private readonly int releaseReferenceCount;

		public ReferenceInfo(string typeName, int unusedReferenceCount, int usingReferenceCount, int acquireReferenceCount, int releaseReferenceCount)
		{
			this.typeName = typeName;
			this.unusedReferenceCount = unusedReferenceCount;
			this.usingReferenceCount = usingReferenceCount;
			this.acquireReferenceCount = acquireReferenceCount;
			this.releaseReferenceCount = releaseReferenceCount;
		}

		public string TypeName
		{
			get
			{
				return typeName;
			}
		}

		public int UnusedReferenceCount
		{
			get
			{
				return unusedReferenceCount;
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
	}
}