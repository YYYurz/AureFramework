//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace AureFramework.Fsm
{
	public enum FsmStatus {
		Running,
		Pause,
	}

	public class Fsm : IFsm{
		private readonly Dictionary<Type, IFsmState> fsmStateDic = new Dictionary<Type, IFsmState>();
		private IFsmState previousFsmState;
		private IFsmState currentFsmState;

		private float durationTime;
		private bool isPause;
		
		public Fsm(IEnumerable<Type> fsmStateTypeList) {
			var interfaceType = typeof(IFsmState);
			foreach (var type in fsmStateTypeList) {
				if (!type.IsSubclassOf(interfaceType)) {
					Debug.LogError($"Fsm : FsmState is not sub class of IFsmState {type.FullName}.");
					continue;
				}

				if (fsmStateDic.ContainsKey(type)) {
					Debug.LogError($"Fsm : FsmState is already exists {type.FullName}.");
					continue;
				}

				var fsmState = Activator.CreateInstance(type, this) as IFsmState;

				fsmStateDic.Add(type, fsmState);
			}

			isPause = false;
		}

		public void Update() {
			if (isPause) {
				return;
			}
			
			currentFsmState.OnUpdate();
		}

		public void Pause() {
			isPause = true;
		}

		public void Resume() {
			isPause = false;
		}

		public void ChangeState<T>() where T : IFsmState {
			var type = typeof(T);
			if (!fsmStateDic.ContainsKey(type)) {
				Debug.LogError($"Fsm : FsmState is not exist in current Fsm {type.FullName}.");
				return;
			}

			previousFsmState = currentFsmState;
			currentFsmState?.OnExit();
			currentFsmState = fsmStateDic[type];
			currentFsmState.OnEnter();
		}

		public IFsmState GetPreviousState() {
			return previousFsmState;
		}

		public IFsmState GetCurrentState() {
			return currentFsmState;
		}

		public FsmStatus GetCurrentStatus() {
			return isPause ? FsmStatus.Pause : FsmStatus.Running;
		}

		public void Destroy() {
			
		}
	} 
}