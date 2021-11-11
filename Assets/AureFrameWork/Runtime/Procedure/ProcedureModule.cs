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

		protected override void Awake() {
			base.Awake();

		}

		public override void Tick() { }
		
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

			var fsmModule = GameMain.GetModule<FsmModule>();
			procedureFsm = fsmModule.CreateFsm(this, procedureList);

			yield return new WaitForEndOfFrame();
			
			StartProcedure();
		}

		public void Init(IFsmModule fsmModule, List<Type> fsmStateList) {
			if (fsmModule == null) {
				Debug.LogError("ProcedureModule : fsmModule is null");
				return;
			}
			
			procedureFsm = fsmModule.CreateFsm(this, fsmStateList);
		}

		public void StartProcedure(){
			if (procedureFsm == null) {
				Debug.LogError("ProcedureModule : procedureFsm is null");
				return;
			}
			
			procedureFsm.ChangeState(entranceProcedure);
		}
	}
}