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

		private float durationTime;
		private bool isPause;

		/// <summary>
		/// 上一个状态
		/// </summary>
		public IFsmState PreviousState { get; private set; }
		
		/// <summary>
		/// 当前状态
		/// </summary>
		public IFsmState CurrentState { get; private set; }
		
		/// <summary>
		/// 状态机处于哪个运行时状态
		/// </summary>
		public FsmStatus Status => isPause ? FsmStatus.Pause : FsmStatus.Running;

		public Fsm(IEnumerable<Type> fsmStateTypeList) {
			var interfaceType = typeof(FsmState);
			foreach (var type in fsmStateTypeList) {
				if (!type.IsSubclassOf(interfaceType)) {
					Debug.LogError($"Fsm : FsmState is not sub class of IFsmState {type.FullName}.");
					continue;
				}

				if (fsmStateDic.ContainsKey(type)) {
					Debug.LogError($"Fsm : FsmState is already exists {type.FullName}.");
					continue;
				}

				var fsmState = Activator.CreateInstance(type) as IFsmState;

				fsmState.OnInit(this);
				fsmStateDic.Add(type, fsmState);
			}

			isPause = false;
		}
		
		/// <summary>
		/// 轮询
		/// </summary>
		public void Update() {
			if (isPause || CurrentState == null) {
				return;
			}
			
			CurrentState.OnUpdate();
		}

		/// <summary>
		/// 暂停状态机轮询
		/// </summary>
		public void Pause() {
			isPause = true;
		}

		/// <summary>
		/// 恢复状态机轮询
		/// </summary>
		public void Resume() {
			isPause = false;
		}

		public void ChangeState<T>(params object[] args) where T : IFsmState {
			var type = typeof(T);
			ChangeState(type, args);
		}

		public void ChangeState(Type fsmType, params object[] args) {
			if (!fsmStateDic.ContainsKey(fsmType)) {
				Debug.LogError($"Fsm : FsmState is not exist in current Fsm {fsmType.FullName}.");
				return;
			}

			PreviousState = CurrentState;
			CurrentState?.OnExit();
			CurrentState = fsmStateDic[fsmType];
			CurrentState.OnEnter(args);
		}

		public void Destroy() {
			
		}
	} 
}