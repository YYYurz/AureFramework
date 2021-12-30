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
	/// 有限状态机状态接口
	/// </summary>
	public interface IFsmState
	{
		void OnInit(IFsm fsmController);

		void OnEnter(params object[] args);

		void OnUpdate();

		void OnExit();

		void ChangeState<T>(params object[] args) where T : IFsmState;
	}
}