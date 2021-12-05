//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using AureFramework.Fsm;
using UnityEngine;

namespace AureFramework.Procedure {
	public class ProcedureModule : AureFrameworkModule, IProcedureModule {
		[SerializeField] private string[] allProcedureTypeNameList;
		[SerializeField] private string entranceProcedureTypeName;
		
		private Type entranceProcedure;
		private IFsm procedureFsm;

		public ProcedureBase CurrentProcedure => (ProcedureBase)procedureFsm.CurrentState;

		public override int Priority => 1;

		public override void Init() {
			
		}

		public override void Tick(float elapseTime, float realElapseTime) {
			
		}
		
		public override void Clear() { }
		
		private IEnumerator Start() {
			var procedureList = new List<Type>();
			foreach (var procedureTypeName in allProcedureTypeNameList) {
				var procedureType = Utility.Assembly.GetType(procedureTypeName);
				if (procedureType == null) {
					Debug.LogError("ProcedureManager : Can not find procedure type.");
					continue;
				}

				if (procedureTypeName == entranceProcedureTypeName) {
					entranceProcedure = procedureType;
				}

				procedureList.Add(procedureType);
			}

			var fsmModule = Aure.GetModule<IFsmModule>();
			procedureFsm = fsmModule.CreateFsm(this, procedureList);

			yield return new WaitForEndOfFrame();
			
			StartProcedure();
		}

		private void StartProcedure(){
			if (procedureFsm == null) {
				Debug.LogError("ProcedureModule : procedureFsm is null");
				return;
			}
			
			procedureFsm.ChangeState(entranceProcedure);
		}
	}
}