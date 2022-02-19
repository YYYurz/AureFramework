//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;

namespace AureFramework.Fsm {
	/// <summary>
	/// 有限状态机接口
	/// </summary>
	public interface IFsm {
		/// <summary>
		/// 上一个状态
		/// </summary>
		IFsmState PreviousState
		{
			get;
		}

		/// <summary>
		/// 当前状态
		/// </summary>
		IFsmState CurrentState
		{
			get;
		}
		
		/// <summary>
		/// 状态机处于哪个运行时状态
		/// </summary>
		FsmStatus Status
		{
			get;
		}
		
		/// <summary>
		/// 轮询
		/// </summary>
		void Update(float elapseTime, float realElapseTime);
		
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
		/// 切换状态
		/// </summary>
		/// <param name="fsmType"> 状态类型 </param>
		/// <param name="args"> 传给下一个状态的参数 </param>
		void ChangeState(Type fsmType, params object[] args);
	}
}