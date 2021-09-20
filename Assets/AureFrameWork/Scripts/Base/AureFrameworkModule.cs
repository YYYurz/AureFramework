//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

namespace AureFramework {
	public abstract class AureFrameworkModule {
		/// <summary>
		/// 轮询
		/// </summary>
		public abstract void Update();

		/// <summary>
		/// 清理数据
		/// </summary>
		public abstract void ClearData();
	}
}