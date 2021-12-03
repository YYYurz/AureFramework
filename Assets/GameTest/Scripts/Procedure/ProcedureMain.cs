//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework.Procedure;
using AureFramework.UI;

namespace GameTest {
	public class ProcedureMain : ProcedureBase {
		public override void OnEnter(params object[] args) {
			base.OnEnter(args);
			// GameEntrance.Lua.DoString("require 'LuaTest'");
			
			
		}

		public override void OnExit() {
			base.OnExit();
			
		}
	}
} 