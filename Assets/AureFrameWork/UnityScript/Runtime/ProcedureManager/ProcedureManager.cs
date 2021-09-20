//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework.Procedure;
using UnityEngine;

namespace AureFramework.Runtime.Procedure {
	public class ProcedureManager : AureFrameworkManager {
		private IProcedureModule procedureModule;

		protected override void Awake() {
			base.Awake();

			procedureModule = GameMain.GetModule<ProcedureModule>();
			if (procedureModule == null) {
				Debug.LogError("ProcedureManager : ProcedureModule is invalid");
				return;
			}
		}
	}
}