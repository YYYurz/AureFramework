//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using UnityEngine;

namespace AureFramework {
	public abstract class AureFrameworkModule : MonoBehaviour {
		/// <summary>
		/// 框架优先级，最小的优先初始化以及轮询
		/// </summary>
		public virtual int Priority => 0;

		/// <summary>
		/// MonoBehaviour注册框架模块
		/// </summary>
		protected virtual void Awake() {
			GameMain.RegisterModule(this);
		}

		/// <summary>
		/// 轮询
		/// </summary>
		public abstract void Tick();

		/// <summary>
		/// 清理
		/// </summary>
		public abstract void Clear();
	}
}