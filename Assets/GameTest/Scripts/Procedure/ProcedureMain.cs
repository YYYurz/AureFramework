//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework.Procedure;
using UnityEngine;

namespace GameTest {
	public class ProcedureMain : ProcedureBase {
		public override void OnEnter(params object[] args) {
			base.OnEnter(args);
			// GameEntrance.Lua.DoString("require 'LuaTest'");
			var objectPool = GameMain.ObjectPool.CreateObjectPool<GameObject>("UIPool", 100, 240);

			var obj = objectPool.Spawn();
			if (obj == null) {
				var uiObj = GameMain.Resource.InstantiateSync("Boom");
				obj = objectPool.Register(uiObj, true);
			}
			
			obj.Target.transform.position = new Vector3(3, 3, 3);
		}

		public override void OnExit() {
			base.OnExit();
			
		}
	}
} 