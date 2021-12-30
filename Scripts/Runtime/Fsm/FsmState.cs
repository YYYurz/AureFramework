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

		public void OnInit(IFsm fsm)
		{
			fsmController = fsm;
		}

		public virtual void OnEnter(params object[] args)
		{
		}

		public virtual void OnUpdate()
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