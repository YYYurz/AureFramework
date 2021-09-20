//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AureFramework.Runtime
{
	public class Aure : MonoBehaviour
	{
		private static readonly List<AureFrameworkManager> ManagerList = new List<AureFrameworkManager>();

		private void Update() {
			GameMain.Update();
		}

		public static T GetManager<T>() where T : AureFrameworkManager {
			return (T) GetManager(typeof(T));
		}

		private static AureFrameworkManager GetManager(Type type) {
			foreach (var manager in ManagerList) {
				if (manager.GetType() == type) {
					return manager;
				}
			}

			return null;
		}

		public static void RegisterManager(AureFrameworkManager manager) {
			if (manager == null) {
				Debug.LogError($"Aure : RegisterManager failed because of manager is null.");
				return;
			}

			if (ManagerList.Contains(manager)) {
				return;
			}
			
			ManagerList.Add(manager);
		}
	}
}