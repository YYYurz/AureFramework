//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System.Collections.Generic;
using AureFramework.Fsm;
using UnityEngine;

namespace AureFramework.Runtime.Fsm {
	public class FsmManager : AureFrameworkManager {
		private IFsmModule fsmModule;

		protected override void Awake() {
			base.Awake();
			
			fsmModule = GameMain.GetModule<FsmModule>();
			if (fsmModule == null) {
				Debug.LogError("FsmManager : FsmModule is invalid");
				return;
			}
		}

		public void CreateFsm<T>(T owner, List<IFsmState> fsmStateList) where T : class {
			fsmModule.CreateFsm(owner, fsmStateList);
		}

		public void ChangeState<T>(T fsmState) where T : IFsmState{
			fsmModule.ChangeState(fsmState);
		}
	}
}