//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using AureFramework.Fsm;
using UnityEngine;

namespace AureFramework.Procedure {
	public class ProcedureModule : AureFrameworkModule, IProcedureModule {
		private IFsm procedureFsm;

		public ProcedureBase CurrentProcedure => (ProcedureBase)procedureFsm.CurrentState;

		public void Init(IFsmModule fsmModule, List<Type> fsmStateList) {
			if (fsmModule == null) {
				Debug.LogError("ProcedureModule : fsmModule is null");
				return;
			}
			
			procedureFsm = fsmModule.CreateFsm(this, fsmStateList);
		}

		public void StartProcedure(Type entranceProcedure){
			if (procedureFsm == null) {
				Debug.LogError("ProcedureModule : procedureFsm is null");
				return;
			}
			
			procedureFsm.ChangeState(entranceProcedure);
		}

		public override void Update() { }
		
		public override void ClearData() { }
	}
}