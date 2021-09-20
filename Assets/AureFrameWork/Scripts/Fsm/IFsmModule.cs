//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System.Collections.Generic;

namespace AureFramework.Fsm {
	public interface IFsmModule {
		/// <summary>
		/// 创建有限状态机
		/// </summary>
		/// <param name="owner"> 持有类 </param>
		/// <param name="fsmStateList"> 状态列表 </param>
		/// <typeparam name="T"></typeparam>
		void CreateFsm<T>(T owner, List<IFsmState> fsmStateList) where T : class;

		/// <summary>
		/// 切换状态
		/// </summary>
		/// <typeparam name="T"></typeparam>
		void ChangeState<T>(T fsmState) where T : IFsmState;
	}
}