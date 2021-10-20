//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
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
		/// 初始化流程模块
		/// </summary>
		/// <param name="fsmModule"> 状态机模块 </param>
		/// <param name="fsmStateList"> 状态列表 </param>
		void Init(IFsmModule fsmModule, List<Type> fsmStateList);

		/// <summary>
		/// 启动流程
		/// </summary>
		/// <typeparam name="T"></typeparam>
		void StartProcedure<T>() where T : IFsmState;
	}
}