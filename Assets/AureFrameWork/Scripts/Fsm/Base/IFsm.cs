//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

namespace AureFramework.Fsm {
	public interface IFsm {
		/// <summary>
		/// 当前状态
		/// </summary>
		IFsmState CurrentState
		{
			get;
		}
		
		/// <summary>
		/// 轮询
		/// </summary>
		void Update();
		
		/// <summary>
		/// 暂停状态机轮询
		/// </summary>
		void Pause();
		
		/// <summary>
		/// 恢复状态机轮询
		/// </summary>
		void Resume();
		
		/// <summary>
		/// 销毁状态机
		/// </summary>
		void Destroy();

		/// <summary>
		/// 切换状态
		/// </summary>
		/// <param name="args"> 传给下一个状态的参数 </param>
		/// <typeparam name="T"></typeparam>
		void ChangeState<T>(params object[] args) where T : IFsmState;
		
		/// <summary>
		/// 获取上一次的状态
		/// </summary>
		/// <returns></returns>
		IFsmState GetPreviousState();

		/// <summary>
		/// 获取当前的状态
		/// </summary>
		/// <returns></returns>
		IFsmState GetCurrentState();

		/// <summary>
		/// 获取当前状态机的状态
		/// </summary>
		/// <returns></returns>
		FsmStatus GetCurrentStatus();
	}
}