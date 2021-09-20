//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace AureFramework
{
	public static class GameMain
	{
		private static readonly List<AureFrameworkModule> ModuleList= new List<AureFrameworkModule>();

		public static T GetModule<T>() where T : AureFrameworkModule => (T)GetModule(typeof(T));

		private static AureFrameworkModule GetModule(Type type) {
			foreach (var manager in ModuleList) {
				if (manager.GetType() == type) {
					return manager;
				}
			}
			
			return CreateModule(type);
		}
		
		
		/// <summary>
		/// 注册游戏框架Manager
		/// </summary>
		/// <param name="moduleType">框架模块类型</param>
		private static AureFrameworkModule CreateModule(Type moduleType) {
			var module = (AureFrameworkModule) Activator.CreateInstance(moduleType);
			if (module == null)
			{
				throw new Exception($"AureFrameWork GameMain : Can not create module {moduleType.FullName}");
			}
			
			ModuleList.Add(module);

			return module;
		}

		public static void Update() {
			foreach (var module in ModuleList) {
				module.Update();
			}
		}

		public static void ShutDown() {
			Application.Quit();
		}
	}
}