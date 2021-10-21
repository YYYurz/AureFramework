//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using AureFramework.Fsm;
using AureFramework.Procedure;
using UnityEngine;

namespace AureFramework.Runtime.Procedure {
	public class ProcedureManager : AureFrameworkManager {
		private IProcedureModule procedureModule;
		private Type entranceProcedure;

		[SerializeField] private string[] allProcedureTypeNameList;
		[SerializeField] private string entranceProcedureTypeName;

		public ProcedureBase CurrentProcedure => procedureModule.CurrentProcedure;

		protected override void Awake() {
			base.Awake();

			procedureModule = GameMain.GetModule<ProcedureModule>();
			if (procedureModule == null) {
				Debug.LogError("ProcedureManager : ProcedureModule is invalid");
			}
		}

		private void Start() {
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

			procedureModule.Init(GameMain.GetModule<FsmModule>(), procedureList);
			procedureModule.StartProcedure(entranceProcedure);
		}
	}
}