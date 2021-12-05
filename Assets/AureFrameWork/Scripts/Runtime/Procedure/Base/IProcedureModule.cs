﻿//------------------------------------------------------------
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
	}
}