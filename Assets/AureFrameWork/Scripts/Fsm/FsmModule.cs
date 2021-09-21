//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace AureFramework.Fsm {
	public sealed class FsmModule : AureFrameworkModule, IFsmModule {
		private readonly Dictionary<object, IFsm> fsmStateDic = new Dictionary<object, IFsm>();

		public void CreateFsm<T>(T owner, IEnumerable<Type> fsmStateList, Type originStateType) where T : class {
			if (fsmStateDic.ContainsKey(owner)) {
				Debug.LogError("FsmModule : The Fsm for this owner already exists.");
				return;
			}
			
			var fsm = new Fsm(fsmStateList, originStateType);
			fsmStateDic.Add(owner, fsm);
		}

		public void DestroyFsm<T>(T owner) where T : class {
			var type = typeof(T);
			if (fsmStateDic.ContainsKey(type)) {
				Debug.LogError("FsmModule : The Fsm for this owner already exists.");
				return;
			}

			var fsm = fsmStateDic[type];
			fsm.Destroy();
			fsmStateDic.Remove(type);
		}

		public override void Update() {
			foreach (var fsm in fsmStateDic) {
				fsm.Value.Update();
			}
		}

		public override void ClearData() {
			foreach (var fsm in fsmStateDic) {
				fsm.Value.Destroy();
			}
			fsmStateDic.Clear();
		}
	}
}