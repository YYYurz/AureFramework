//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
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

		/// <summary>
		/// 创建有限状态机
		/// </summary>
		/// <param name="owner"> 持有类 </param>
		/// <param name="fsmStateList"> 状态列表 </param>
		/// <param name="originStateType"> 起始状态 </param>
		/// <typeparam name="T"></typeparam>
		public void CreateFsm<T>(T owner, IEnumerable<Type> fsmStateList, Type originStateType) where T : class {
			fsmModule.CreateFsm(owner, fsmStateList, originStateType);
		}

		/// <summary>
		/// 销毁有限状态机
		/// </summary>
		/// <param name="owner"> 持有类 </param>
		/// <typeparam name="T"></typeparam>
		void DestroyFsm<T>(T owner) where T : class {
			fsmModule.DestroyFsm<T>(owner);
		}
	}
}