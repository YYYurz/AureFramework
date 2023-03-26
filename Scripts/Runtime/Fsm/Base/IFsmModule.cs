//------------------------------------------------------------
// AureFramework
// Developed By YYYurz
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace AureFramework.Fsm
{
	/// <summary>
	/// 有限状态机模块接口
	/// </summary>
	public interface IFsmModule
	{
		/// <summary>
		/// 创建有限状态机
		/// </summary>
		/// <param name="owner"> 持有类 </param>
		/// <param name="fsmStateList"> 状态列表 </param>
		/// <param name="userData"> 用户数据 </param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		IFsm CreateFsm<T>(T owner, List<Type> fsmStateList, params object[] userData) where T : class;

		/// <summary>
		/// 销毁有限状态机
		/// </summary>
		/// <param name="owner"> 持有类 </param>
		/// <typeparam name="T"></typeparam>
		void DestroyFsm<T>(T owner) where T : class;
	}
}