//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
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
	/// 有限状态机模块
	/// </summary>
	public sealed class FsmModule : AureFrameworkModule, IFsmModule
	{
		private readonly Dictionary<object, IFsm> fsmStateDic = new Dictionary<object, IFsm>();

		/// <summary>
		/// 模块优先级，最小的优先轮询
		/// </summary>
		public override int Priority => 1;

		/// <summary>
		/// 模块初始化，只在第一次被获取时调用一次
		/// </summary>
		public override void Init()
		{
		}

		/// <summary>
		/// 框架轮询
		/// </summary>
		/// <param name="elapseTime"> 距离上一帧的流逝时间，秒单位 </param>
		/// <param name="realElapseTime"> 距离上一帧的真实流逝时间，秒单位 </param>
		public override void Tick(float elapseTime, float realElapseTime)
		{
			foreach (var fsm in fsmStateDic)
			{
				fsm.Value.Update(elapseTime, realElapseTime);
			}
		}

		/// <summary>
		/// 框架清理
		/// </summary>
		public override void Clear()
		{
			foreach (var fsm in fsmStateDic)
			{
				fsm.Value.Destroy();
			}

			fsmStateDic.Clear();
		}

		/// <summary>
		/// 创建有限状态机
		/// </summary>
		/// <param name="owner"> 持有类 </param>
		/// <param name="fsmStateList"> 状态列表 </param>
		/// <param name="userData"> 用户数据 </param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public IFsm CreateFsm<T>(T owner, List<Type> fsmStateList, params object[] userData) where T : class
		{
			if (fsmStateDic.ContainsKey(owner))
			{
				Debug.LogError("FsmModule : The Fsm for this owner already exists.");
				return null;
			}

			var fsm = new Fsm(fsmStateList, userData);
			fsmStateDic.Add(owner, fsm);

			return fsm;
		}

		/// <summary>
		/// 销毁有限状态机
		/// </summary>
		/// <param name="owner"> 持有类 </param>
		/// <typeparam name="T"></typeparam>
		public void DestroyFsm<T>(T owner) where T : class
		{
			if (!fsmStateDic.ContainsKey(owner))
			{
				Debug.LogError("FsmModule : The Fsm for this owner not exists.");
				return;
			}

			var fsm = fsmStateDic[owner];
			fsm.Destroy();
			fsmStateDic.Remove(owner);
		}
	}
}