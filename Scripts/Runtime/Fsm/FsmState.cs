//------------------------------------------------------------
// AureFramework
// Developed By YYYurz
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

namespace AureFramework.Fsm
{
	/// <summary>
	/// 状态基类
	/// </summary>
	public abstract class FsmState : IFsmState
	{
		private IFsm fsmController;

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="fsm"> 状态机控制类 </param>
		/// <param name="userData"> 用户数据 </param>
		public virtual void OnInit(IFsm fsm, params object[] userData)
		{
			fsmController = fsm;
		}

		/// <summary>
		/// 进入状态
		/// </summary>
		/// <param name="args"> 切换状态时传入的参数 </param>
		public virtual void OnEnter(params object[] args)
		{
		}

		/// <summary>
		/// 轮询
		/// </summary>
		/// <param name="elapseTime"> 距离上一帧的流逝时间，秒单位 </param>
		/// <param name="realElapseTime"> 距离上一帧的真实流逝时间，秒单位 </param>
		public virtual void OnUpdate(float elapseTime, float realElapseTime)
		{
		}

		/// <summary>
		/// 暂停
		/// </summary>
		public virtual void OnPause()
		{
		}

		/// <summary>
		/// 恢复
		/// </summary>
		public virtual void OnResume()
		{
		}

		/// <summary>
		/// 退出状态
		/// </summary>
		public virtual void OnExit()
		{
		}

		/// <summary>
		/// 切换状态
		/// </summary>
		/// <param name="args"> 传给下一状态的参数 </param>
		/// <typeparam name="T"> 下一状态 </typeparam>
		public void ChangeState<T>(params object[] args) where T : IFsmState
		{
			fsmController.ChangeState<T>(args);
		}
	}
}