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

		public IFsm CreateFsm<T>(T owner, List<Type> fsmStateList) where T : class {
			if (fsmStateDic.ContainsKey(owner)) {
				Debug.LogError("FsmModule : The Fsm for this owner already exists.");
				return null;
			}
			
			var fsm = new Fsm(fsmStateList);
			fsmStateDic.Add(owner, fsm);
			
			return fsm;
		}

		public void DestroyFsm<T>(T owner) where T : class {
			var type = typeof(T);
			if (!fsmStateDic.ContainsKey(type)) {
				Debug.LogError("FsmModule : The Fsm for this owner not exists.");
				return;
			}

			var fsm = fsmStateDic[type];
			fsm.Destroy();
			fsmStateDic.Remove(type);
		}

		public override void Tick() {
			foreach (var fsm in fsmStateDic) {
				fsm.Value.Update();
			}
		}

		public override void Clear() {
			foreach (var fsm in fsmStateDic) {
				fsm.Value.Destroy();
			}
			fsmStateDic.Clear();
		}
	}
}