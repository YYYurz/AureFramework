//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework.Procedure;
using UnityEngine;

namespace GameTest {
	public class ProcedureLaunch : ProcedureBase {
		private float timeRecord = 0; 
		
		public override void OnEnter(params object[] args) {
			base.OnEnter(args);

			timeRecord = 0;
			
			Debug.Log("LaunchProcedure : OnEnter");
		}

		public override void OnUpdate() {
			base.OnUpdate();

			timeRecord += Time.deltaTime;
			if (timeRecord > 5) {
				ChangeState<ProcedureMain>();
			}
		}

		public override void OnExit(params object[] args) {
			base.OnExit(args);
			
			Debug.Log("LaunchProcedure : OnExit");
		}
	}
}