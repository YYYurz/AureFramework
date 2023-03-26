//------------------------------------------------------------
// AureFramework
// Developed By YYYurz
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using UnityEngine;

namespace AureFramework
{
	public abstract class AureFrameworkModule : MonoBehaviour
	{
		/// <summary>
		/// 模块优先级，最小的优先轮询
		/// </summary>
		public virtual int Priority => 0;

		/// <summary>
		/// MonoBehaviour自动注册框架模块
		/// </summary>
		private void Awake()
		{
			Aure.RegisterModule(this);
		}

		/// <summary>
		/// 模块初始化，只在第一次被获取时调用一次
		/// </summary>
		public abstract void Init();

		/// <summary>
		/// 框架轮询
		/// </summary>
		/// <param name="elapseTime"> 距离上一帧的流逝时间，秒单位 </param>
		/// <param name="realElapseTime"> 距离上一帧的真实流逝时间，秒单位 </param>
		public abstract void Tick(float elapseTime, float realElapseTime);

		/// <summary>
		/// 框架清理
		/// </summary>
		public abstract void Clear();
	}
}