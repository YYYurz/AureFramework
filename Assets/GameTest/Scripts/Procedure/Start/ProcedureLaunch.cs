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

namespace GameTest {
	public class ProcedureLaunch : ProcedureBase {

		public override void OnEnter(params object[] args) {
			base.OnEnter(args);
			// GameEntrance.Lua.DoString("require 'LuaTest'");

			GameEntrance.Event.Subscribe(LoadSuccessEventArgs.Id, OnLoadSuccess1);
			GameEntrance.Event.Subscribe(LoadSuccessEventArgs.Id, OnLoadSuccess2);
			GameEntrance.Event.Unsubscribe(LoadSuccessEventArgs.Id, OnLoadSuccess2);

			GameEntrance.Event.Fire(this, LoadSuccessEventArgs.Create("哇 我丢"));
		}
		
		public override void OnUpdate() {
			base.OnUpdate();

			ChangeState<ProcedureMain>();
		}

		public override void OnExit() {
			base.OnExit();
			
			GameEntrance.Event.Unsubscribe(LoadSuccessEventArgs.Id, OnLoadSuccess1);
			GameEntrance.Event.Unsubscribe(LoadSuccessEventArgs.Id, OnLoadSuccess2);
		}

		private void OnLoadSuccess1(object sender, GameEventArgs e) {
			if (e == null) {
				Debug.LogError("GameEventArgs == null");
				return;
			}

			var args = (LoadSuccessEventArgs) e;
			Debug.Log("Number1   " + args.Content);
		}
		
		private void OnLoadSuccess2(object sender, GameEventArgs e) {
			if (e == null) {
				Debug.LogError("GameEventArgs == null");
				return;
			}

			var args = (LoadSuccessEventArgs) e;
			Debug.Log("Number2   " + args.Content);
		}
	}
}