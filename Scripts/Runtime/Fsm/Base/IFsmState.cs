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
	/// 有限状态机状态接口
	/// </summary>
	public interface IFsmState
	{
		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="fsmController"> 状态机控制类 </param>
		/// <param name="userData"> 用户数据 </param>
		void OnInit(IFsm fsmController, params object[] userData);

		/// <summary>
		/// 进入状态
		/// </summary>
		/// <param name="args"> 切换状态时传入的参数 </param>
		void OnEnter(params object[] args);

		/// <summary>
		/// 轮询
		/// </summary>
		/// <param name="elapseTime"> 距离上一帧的流逝时间，秒单位 </param>
		/// <param name="realElapseTime"> 距离上一帧的真实流逝时间，秒单位 </param>
		void OnUpdate(float elapseTime, float realElapseTime);

		/// <summary>
		/// 暂停
		/// </summary>
		void OnPause();

		/// <summary>
		/// 恢复
		/// </summary>
		void OnResume();

		/// <summary>
		/// 退出状态
		/// </summary>
		void OnExit();

		/// <summary>
		/// 切换状态
		/// </summary>
		/// <param name="args"> 传给下一状态的参数 </param>
		/// <typeparam name="T"> 下一状态 </typeparam>
		void ChangeState<T>(params object[] args) where T : IFsmState;
	}
}