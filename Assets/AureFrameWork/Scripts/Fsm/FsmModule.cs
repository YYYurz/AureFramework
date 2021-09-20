//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace AureFramework.Fsm {
	public sealed class FsmModule : AureFrameworkModule, IFsmModule {
		private readonly Dictionary<object, List<IFsmState>> dicFsmState = new Dictionary<object, List<IFsmState>>();
		private IFsmState previousFsmState;
		private IFsmState currentFsmState;

		private float durationTime;

		public void CreateFsm<T>(T owner, List<IFsmState> fsmStateList) where T : class{
			if (dicFsmState.ContainsKey(owner)) {
				throw new Exception("FsmModule : The Fsm for this owner already exists.");
			}
			
			dicFsmState.Add(owner, fsmStateList);
		}

		public void ChangeState<T>(T fsmState) where T : IFsmState{
			
		}

		public override void Update() {
		}

		public override void ClearData() {
		}
	}
}