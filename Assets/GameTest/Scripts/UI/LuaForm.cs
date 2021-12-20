//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using AureFramework.UI;
using UnityEngine;
using XLua;

namespace GameTest {
	/// <summary>
	/// Lua界面
	/// </summary>
	public sealed class LuaForm : UIFormBase {
		private string uiName;

		private LuaTable luaScriptTable;
		// private UIFormOpenDataInfo formDataInfo { get; set; }

		public override void OnInit(object userData) {
			base.OnInit(userData);
			// formDataInfo = userData as UIFormOpenDataInfo;
			// if (formDataInfo == null)
			// {
			//     Debug.LogError("LuaForm Open Error! invalid userData!");
			//     return;
			// }

			luaScriptTable = (LuaTable) GameMain.Lua.CallLuaFunction("", "New", new[] {typeof(LuaTable)})[0];

			if (luaScriptTable != null) {
				luaScriptTable.Set("transform", transform);
				luaScriptTable.Set("gameObject", gameObject);
				GameMain.Lua.CallLuaFunction(luaScriptTable, "OnCreate", luaScriptTable);
			}
		}

		public override void OnOpen(object userData) {
			base.OnOpen(userData);

			formDataInfo = userData as UIFormOpenDataInfo;
			if (formDataInfo == null) {
				Debug.LogError("LuaForm Open Error! invalid userData!");
				return;
			}

			if (formDataInfo.UserData as string == "Preload") {
				return;
			}

			if (luaScriptTable != null) {
				GameMain.Lua.CallLuaFunction(luaScriptTable, "OnOpen", luaScriptTable);
			}
		}

		public override void OnClose() {
			if (luaScriptTable != null) {
				GameMain.Lua.CallLuaFunction(luaScriptTable, "OnClose", luaScriptTable);
			}

			base.OnClose();
		}

		public override void OnDestroy() {
			if (luaScriptTable == null) return;
			GameMain.Lua.CallLuaFunction(luaScriptTable, "OnDestroy", luaScriptTable);
			luaScriptTable.Dispose();
			luaScriptTable = null;
		}
	}
}