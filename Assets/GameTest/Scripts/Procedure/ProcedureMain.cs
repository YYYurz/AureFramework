//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System.Collections.Generic;
using AureFramework.Event;
using AureFramework.ObjectPool;
using AureFramework.Procedure;
using UnityEngine;

namespace GameTest {
	public class ProcedureMain : ProcedureBase {
		public override void OnEnter(params object[] args) {
			base.OnEnter(args);

			var a = new Vector2(1, -1);
			var b = new Vector2(1, 0);

			var test = Vector2.Angle(a, b);
			
			Debug.Log(test);
		}

		public override void OnUpdate() {
			base.OnUpdate();

		}
	}
} 