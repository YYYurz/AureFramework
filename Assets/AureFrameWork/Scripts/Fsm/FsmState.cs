//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

namespace AureFramework.Fsm {
	public abstract class FsmState : IFsmState {
		private readonly IFsm fsmController;
		
		public FsmState(IFsm fsmController) {
			this.fsmController = fsmController;
		}
		
		public virtual void OnEnter(params object[] args) { }
		
		public virtual void OnUpdate() { }
		
		public virtual void OnExit(params object[] args) { }
		
		public void ChangeState<T>() where T : IFsmState{
			fsmController.ChangeState<T>();
		}
	}
}