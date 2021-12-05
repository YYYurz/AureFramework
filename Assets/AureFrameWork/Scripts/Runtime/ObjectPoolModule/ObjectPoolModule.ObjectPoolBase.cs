//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

namespace AureFramework.ObjectPool {
	public sealed partial class ObjectPoolModule : AureFrameworkModule, IObjectPoolModule {
		private abstract class ObjectPoolBase {
			/// <summary>
			/// 轮询
			/// </summary>
			/// <param name="elapseTime"></param>
			/// <param name="realElapseTime"></param>
			public abstract void Update(float elapseTime, float realElapseTime);

			/// <summary>
			/// 销毁
			/// </summary>
			public abstract void ShutDown();
		}
	}
}