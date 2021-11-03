//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework.Procedure;
using UnityEngine;

namespace GameTest {
	public class ProcedureMain : ProcedureBase {
		public override void OnEnter(params object[] args) {
			base.OnEnter(args);
			
			Debug.Log("ProcedureMain : OnEnter");
		}

		public override void OnExit() {
			base.OnExit();
			
			Debug.Log("ProcedureMain : OnEnter");
		}
	}
}