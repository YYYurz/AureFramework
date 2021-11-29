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

			GameEntrance.Event.Subscribe<LoadSuccessEventArgs>(Func1);
			GameEntrance.Event.Subscribe<LoadFailedEventArgs>(Func2);
			GameEntrance.Event.Subscribe<LoadFailedEventArgs>(Func2);

			GameEntrance.Event.Fire(this, LoadSuccessEventArgs.Create("Ha"));
		}

		public override void OnExit() {
			base.OnExit();
			
			GameEntrance.Event.Unsubscribe<LoadSuccessEventArgs>(Func1);
			GameEntrance.Event.Unsubscribe<LoadFailedEventArgs>(Func2);
		}

		private void Func1(object sender, GameEventArgs e) {
			var args = (LoadSuccessEventArgs) e;
			Debug.Log(args.Content);
		}
		
		private void Func2(object sender, GameEventArgs e) {
			var args = (LoadSuccessEventArgs) e;
			Debug.Log(args.Content);
		}
	}
}