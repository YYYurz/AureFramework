//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

namespace AureFramework.Fsm {
	public interface IFsmState {
		void OnInit(IFsm fsmController);
		
		void OnEnter(params object[] args);
		
		void OnUpdate();
		
		void OnExit(params object[] args);

		void ChangeState<T>(params object[] args) where T : IFsmState;
	}
}