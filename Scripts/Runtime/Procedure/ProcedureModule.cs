//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AureFramework.Fsm;
using AureFramework.Utility;
using UnityEngine;

namespace AureFramework.Procedure
{
	/// <summary>
	/// 流程模块
	/// </summary>
	public class ProcedureModule : AureFrameworkModule, IProcedureModule
	{
		private Type entranceProcedure;
		private IFsm procedureFsm;

		[SerializeField] private string[] allProcedureTypeNameList;
		[SerializeField] private string entranceProcedureTypeName;

		/// <summary>
		/// 当前流程
		/// </summary>
		public ProcedureBase CurrentProcedure => (ProcedureBase) procedureFsm.CurrentState;

		/// <summary>
		/// 模块优先级，最小的优先轮询
		/// </summary>
		public override int Priority => -1;

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
		}

		/// <summary>
		/// 框架清理
		/// </summary>
		public override void Clear()
		{
			procedureFsm = null;
		}

		/// <summary>
		/// 切换流程
		/// </summary>
		/// <param name="args"> 传给下一个流程的参数 </param>
		/// <typeparam name="T"></typeparam>
		public void ChangeProcedure<T>(params object[] args) where T : ProcedureBase
		{
			var procedureTypeName = typeof(T).FullName;
			if (!allProcedureTypeNameList.Contains(procedureTypeName))
			{
				Debug.LogError("ProcedureModule : Can not find procedure type.");
				return;
			}

			procedureFsm.ChangeState<T>(args);
		}

		/// <summary>
		/// 切换流程
		/// </summary>
		/// <param name="procedureType"> 流程类型 </param>
		/// <param name="args"> 传给下一个流程的参数 </param>
		public void ChangeProcedure(Type procedureType, params object[] args)
		{
			if (!allProcedureTypeNameList.Contains(procedureType.FullName))
			{
				Debug.LogError("ProcedureModule : Can not find procedure type.");
				return;
			}

			procedureFsm.ChangeState(procedureType, args);
		}

		private IEnumerator Start()
		{
			var procedureList = new List<Type>();
			foreach (var procedureTypeName in allProcedureTypeNameList)
			{
				var procedureType = Assembly.GetType(procedureTypeName);
				if (procedureType == null)
				{
					Debug.LogError("ProcedureModule : Can not find procedure type.");
					continue;
				}

				if (procedureTypeName == entranceProcedureTypeName)
				{
					entranceProcedure = procedureType;
				}

				procedureList.Add(procedureType);
			}

			procedureFsm = Aure.GetModule<IFsmModule>().CreateFsm(this, procedureList);

			yield return new WaitForEndOfFrame();

			StartProcedure();
		}

		private void StartProcedure()
		{
			if (procedureFsm == null)
			{
				Debug.LogError("ProcedureModule : procedureFsm is null.");
				return;
			}

			procedureFsm.ChangeState(entranceProcedure);
		}
	}
}