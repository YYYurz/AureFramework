//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace AureFramework
{
	public static class GameMain
	{
		private static readonly LinkedList<AureFrameworkModule> ModuleLinked= new LinkedList<AureFrameworkModule>();
		
		/// <summary>
		/// 注册框架模块
		/// </summary>
		/// <param name="module"></param>
		public static void RegisterModule(AureFrameworkModule module) {
			if (ModuleLinked.Contains(module)) {
				Debug.LogError($"AureFramework GameMain : Module is exists, can not register it again. module : {module.GetType().FullName}");
				return;
			}

			var curNode = ModuleLinked.First;
			while (curNode != null)
			{
				if (module.Priority > curNode.Value.Priority)
				{
					break;
				}

				curNode = curNode.Next;
			}

			if (curNode != null)
			{
				ModuleLinked.AddBefore(curNode, module);
			}
			else
			{
				ModuleLinked.AddLast(module);
			}
		}
		
		/// <summary>
		/// 获取框架组件
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static T GetModule<T>() where T : AureFrameworkModule {
			foreach (var module in ModuleLinked) {
				if (module.GetType() == typeof(T)) {
					return (T)module;
				}
			}
			
			return null;
		}

		/// <summary>
		/// 框架轮询
		/// </summary>
		public static void Update() {
			foreach (var module in ModuleLinked) {
				module.Tick();
			}
		}

		public static void ShutDown() {
			foreach (var module in ModuleLinked) {
				module.Clear();
			}
			ModuleLinked.Clear();
			Application.Quit();
		}
	}
}