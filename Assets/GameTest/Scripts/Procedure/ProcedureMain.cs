//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using AureFramework;
using AureFramework.ObjectPool;
using AureFramework.Procedure;
using AureFramework.UI;
using UnityEngine;

namespace GameTest {
	public class UIObject : AureObjectBase {
		public static UIObject Create(string name, GameObject obj) {
			var uiObject = GameEntrance.ReferencePool.Acquire<UIObject>();
			uiObject.Init(name, obj);

			return uiObject;
		}	
	}
	
	public class ProcedureMain : ProcedureBase {
		public override void OnEnter(params object[] args) {
			base.OnEnter(args);
			// GameEntrance.Lua.DoString("require 'LuaTest'");
			var objectPool = GameEntrance.ObjectPool.CreateObjectPool<UIObject>(10, 240);


			var uiObj = objectPool.Spawn();
			if (uiObj == null) {
				var ui = GameEntrance.Resource.InstantiateSync("Boom");
				uiObj = UIObject.Create("ui", ui);
			}
		}

		public override void OnExit() {
			base.OnExit();
			
		}
	}


	interface IObject {
			
	}
	
	
} 