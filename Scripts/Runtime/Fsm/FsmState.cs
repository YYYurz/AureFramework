//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
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

		public virtual void OnInit(IFsm fsm, params object[] userData)
		{
			fsmController = fsm;
		}

		public virtual void OnEnter(params object[] args)
		{
		}

		public virtual void OnUpdate(float elapseTime, float realElapseTime)
		{
		}

		public virtual void OnExit()
		{
		}

		public void ChangeState<T>(params object[] args) where T : IFsmState
		{
			fsmController.ChangeState<T>(args);
		}
	}
}