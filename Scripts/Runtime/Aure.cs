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

namespace AureFramework
{
	/// <summary>
	/// 框架主模块
	/// </summary>
	public class Aure : MonoBehaviour
	{
		private static readonly List<AureFrameworkModule> RegisteredModuleList = new List<AureFrameworkModule>();
		private static readonly LinkedList<AureFrameworkModule> ModuleLinked = new LinkedList<AureFrameworkModule>();
		private const string frameworkVersion = "0.0.0.1";
		private const string gameVersion = "0.0.0.1";

		private void Awake()
		{
			Debug.Log($"Aure Framework Version:{frameworkVersion}");
			Debug.Log($"Game Version:{gameVersion}");
			Debug.Log($"Unity Version:{Application.unityVersion}");
		}

		/// <summary>
		/// 框架轮询
		/// </summary>
		public void Update()
		{
			foreach (var module in ModuleLinked)
			{
				module.Tick(Time.deltaTime, Time.unscaledDeltaTime);
			}
		}

		private void OnDestroy()
		{
			foreach (var module in ModuleLinked)
			{
				module.Clear();
			}

			ModuleLinked.Clear();
			RegisteredModuleList.Clear();
		}

		public static void ShutDown()
		{
			Application.Quit();
		}

		/// <summary>
		/// 注册框架模块
		/// </summary>
		/// <param name="module"> 框架模块 </param>
		public static void RegisterModule(AureFrameworkModule module)
		{
			if (RegisteredModuleList.Contains(module))
			{
				Debug.LogError($"GameMain : Module is exists, can not register it again. module : {module.GetType().FullName}");
				return;
			}

			RegisteredModuleList.Add(module);
		}

		/// <summary>
		/// 获取框架组件
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static T GetModule<T>() where T : class
		{
			var interfaceType = typeof(T);
			if (!interfaceType.IsInterface)
			{
				Debug.LogError($"GameMain : Module must be interface {interfaceType.FullName}.");
				return null;
			}

			return InternalGetModule(typeof(T)) as T;
		}

		private static AureFrameworkModule InternalGetModule(Type moduleType)
		{
			foreach (var module in ModuleLinked)
			{
				if (moduleType.IsInstanceOfType(module))
				{
					return module;
				}
			}

			return InternalActivateModule(moduleType);
		}

		private static AureFrameworkModule InternalActivateModule(Type moduleType)
		{
			AureFrameworkModule tempModule = null;
			foreach (var module in RegisteredModuleList)
			{
				if (moduleType.IsInstanceOfType(module))
				{
					tempModule = module;
					break;
				}
			}

			if (tempModule != null)
			{
				var curNode = ModuleLinked.First;
				while (curNode != null)
				{
					if (tempModule.Priority > curNode.Value.Priority)
					{
						break;
					}

					curNode = curNode.Next;
				}

				if (curNode != null)
				{
					ModuleLinked.AddBefore(curNode, tempModule);
				}
				else
				{
					ModuleLinked.AddLast(tempModule);
				}

				tempModule.Init();
				RegisteredModuleList.Remove(tempModule);
			}
			else
			{
				Debug.LogError($"GameMain : This module has not been registered {moduleType.FullName}.");
			}

			return tempModule;
		}
	}
}