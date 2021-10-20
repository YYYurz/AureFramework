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
using AureFramework.Procedure;
using UnityEngine;

namespace AureFramework.Runtime.Procedure {
	public class ProcedureManager : AureFrameworkManager {
		private IProcedureModule procedureModule;
		private ProcedureBase entranceProcedure;
		private Type test;

		[SerializeField] private List<string> procedureNameList;
		[SerializeField] private string entranceProcedureName;

		public ProcedureBase CurrentProcedure => procedureModule.CurrentProcedure;

		protected override void Awake() {
			base.Awake();

			procedureModule = GameMain.GetModule<ProcedureModule>();
			if (procedureModule == null) {
				Debug.LogError("ProcedureManager : ProcedureModule is invalid");
			}
		}

		private void Start() {
			var procedureList = new List<ProcedureBase>();
			foreach (var procedureName in procedureNameList) {
				var procedureType = Utility.Assembly.GetType(procedureName);
				if (procedureType == null) {
					Debug.LogError("ProcedureManager : Can not find procedure type.");
					continue;
				}

				if (procedureName == entranceProcedureName) {
					procedureModule.StartProcedure(test);
				}

			}
		}
	}
}