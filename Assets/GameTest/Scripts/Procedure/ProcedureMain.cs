//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using AureFramework.Event;
using AureFramework.Procedure;
using AureFramework.Resource;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameTest {
	public class ProcedureMain : ProcedureBase {
		public override void OnEnter(params object[] args) {
			base.OnEnter(args);
			// GameEntrance.Lua.DoString("require 'LuaTest'");

			GameEntrance.Event.Fire(this, LoadAssetFailedEventArgs.Create("Ha"));

			A<LoadAssetSuccessEventArgs>.num = 2;
			A<LoadAssetFailedEventArgs>.num = 3;
			var a = A<LoadAssetSuccessEventArgs>.num;
			var b = A<LoadAssetSuccessEventArgs>.num;
			var c = A<LoadAssetFailedEventArgs>.num;
			Debug.Log(a);
			Debug.Log(b);
			Debug.Log(c);
		}

		public override void OnExit() {
			base.OnExit();
			
		}
	}

	public class A<T> where T : GameEventArgs {
		public static int num = 1;
	}
} 