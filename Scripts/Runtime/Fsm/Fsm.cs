//------------------------------------------------------------
// AureFramework
// Developed By YYYurz
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace AureFramework.Fsm
{
	/// <summary>
	/// 状态机的状态
	/// </summary>
	public enum FsmStatus
	{
		Running,
		Pause,
	}

	/// <summary>
	/// 有限状态机
	/// </summary>
	public class Fsm : IFsm
	{
		private readonly Dictionary<Type, IFsmState> fsmStateDic = new Dictionary<Type, IFsmState>();
		private bool isPause;

		public Fsm(IEnumerable<Type> fsmStateTypeList, params object[] userData)
		{
			var interfaceType = typeof(FsmState);
			foreach (var type in fsmStateTypeList)
			{
				if (!type.IsSubclassOf(interfaceType))
				{
					Debug.LogError($"Fsm : FsmState is not sub class of IFsmState {type.FullName}.");
					continue;
				}

				if (fsmStateDic.ContainsKey(type))
				{
					Debug.LogError($"Fsm : FsmState is already exists {type.FullName}.");
					continue;
				}

				var fsmState = Activator.CreateInstance(type) as IFsmState;

				fsmState.OnInit(this, userData);
				fsmStateDic.Add(type, fsmState);
			}

			isPause = false;
		}

		/// <summary>
		/// 上一个状态
		/// </summary>
		public IFsmState PreviousState
		{
			get;
			private set;
		}

		/// <summary>
		/// 当前状态
		/// </summary>
		public IFsmState CurrentState
		{
			get;
			private set;
		}

		/// <summary>
		/// 状态机处于哪个运行时状态
		/// </summary>
		public FsmStatus Status
		{
			get
			{
				return isPause ? FsmStatus.Pause : FsmStatus.Running;
			}
		}

		/// <summary>
		/// 轮询
		/// </summary>
		public void Update(float elapseTime, float realElapseTime)
		{
			if (isPause || CurrentState == null)
			{
				return;
			}

			CurrentState.OnUpdate(elapseTime, realElapseTime);
		}

		/// <summary>
		/// 暂停状态机轮询
		/// </summary>
		public void Pause()
		{
			isPause = true;
			CurrentState?.OnPause();
		}

		/// <summary>
		/// 恢复状态机轮询
		/// </summary>
		public void Resume()
		{
			isPause = false;
			CurrentState?.OnResume();
		}

		/// <summary>
		/// 切换状态
		/// </summary>
		/// <param name="args"> 传给下一个状态的参数 </param>
		/// <typeparam name="T"></typeparam>
		public void ChangeState<T>(params object[] args) where T : IFsmState
		{
			var type = typeof(T);
			ChangeState(type, args);
		}

		/// <summary>
		/// 切换状态
		/// </summary>
		/// <param name="fsmType"> 状态类型 </param>
		/// <param name="args"> 传给下一个状态的参数 </param>
		public void ChangeState(Type fsmType, params object[] args)
		{
			if (!fsmStateDic.ContainsKey(fsmType))
			{
				Debug.LogError($"Fsm : FsmState is not exist in current Fsm {fsmType.FullName}.");
				return;
			}
			
			PreviousState = CurrentState;
			CurrentState?.OnExit();
			CurrentState = fsmStateDic[fsmType];
			CurrentState.OnEnter(args);
		}

		public void Destroy()
		{
			CurrentState?.OnExit();
			PreviousState = null;
			CurrentState = null;
			fsmStateDic.Clear();
		}
	}
}