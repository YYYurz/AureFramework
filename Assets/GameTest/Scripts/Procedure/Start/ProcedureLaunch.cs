﻿//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework.Procedure;
using UnityEngine;

namespace GameTest {
	public class ProcedureLaunch : ProcedureBase {
		private GameObject asd;
		
		public override void OnEnter(params object[] args) {
			base.OnEnter(args);

			GameEntrance.Resource.LoadAssetAsync<GameObject>("Boom", obj => {
				Debug.Log("Load Success!!");
				// Addressables.Release(obj);
			});

			// GameEntrance.Lua.DoString("LuaTxt");
			GameEntrance.Lua.DoString("require 'LuaTxt'");

			Debug.Log("LaunchProcedure : OnEnter");
		}

		public override void OnUpdate() {
			base.OnUpdate();

			ChangeState<ProcedureMain>();
		}

		public override void OnExit() {
			base.OnExit();
			
			Debug.Log("LaunchProcedure : OnExit");
		}
	}
}