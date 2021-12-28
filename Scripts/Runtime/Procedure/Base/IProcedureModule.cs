//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using AureFramework.Fsm;

namespace AureFramework.Procedure {
	public interface IProcedureModule {
		ProcedureBase CurrentProcedure
		{
			get;
		}
		
		/// <summary>
		/// 切换流程
		/// </summary>
		/// <param name="args"> 传给下一个流程的参数 </param>
		/// <typeparam name="T"></typeparam>
		void ChangeProcedure<T>(params object[] args) where T : ProcedureBase;
		
		/// <summary>
		/// 切换流程
		/// </summary>
		/// <param name="procedureType"> 流程类型 </param>
		/// <param name="args"> 传给下一个流程的参数 </param>
		void ChangeProcedure(Type procedureType, params object[] args);
	}
}