//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using AureFramework.Event;
using AureFramework.Procedure;
using AureFramework.Resource;
using UnityEngine;

namespace GameTest {
	public class ProcedureLaunch : ProcedureBase {
		public override void OnEnter(params object[] args) {
			base.OnEnter(args);

		}
		
		public override void OnUpdate() {
			base.OnUpdate();

			ChangeState<ProcedureChangeScene>("TestScene");
		}

		public override void OnExit() {
			base.OnExit();
			
		}
	}
}