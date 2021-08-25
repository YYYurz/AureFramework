//------------------------------------------------------------
// No Framework
// Develop By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace AureFrameWork.Runtime
{
	public static class GameMain
	{
		private static readonly List<AureFrameworkManager> ManagerList= new List<AureFrameworkManager>();

		public static T GetManager<T>() where T : AureFrameworkManager => (T)GetManager(typeof(T));

		private static AureFrameworkManager GetManager(Type type) {
			foreach (var manager in ManagerList) {
				if (manager.GetType() == type) {
					return manager;
				}
			}
			
			Debug.LogError($"AureFrameWork GameMain : manager type {type.FullName} is not exist");
			return null;
		}
		
		
		/// <summary>
		/// 注册游戏框架Manager
		/// </summary>
		/// <param name="frameWorkManager">要注册的游戏框架Manager</param>
		internal static void RegisterManager(AureFrameworkManager frameWorkManager)
		{
			if (frameWorkManager == null)
			{
				Debug.Log("Game Framework component is invalid");
				return;
			}

			var type = frameWorkManager.GetType();
			foreach (var manager in ManagerList) {
				if (manager.GetType() == type) {
					Debug.LogError($"AureFrameWork GameMain : manager type {type.FullName} is already exist");
					return;
				}
			}
			
			ManagerList.Add(frameWorkManager);
		}

		public static void ShutDown() {
			Application.Quit();
		}
	}
}