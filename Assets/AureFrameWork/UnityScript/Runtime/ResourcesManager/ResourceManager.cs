//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using AureFramework.Resource;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AureFramework.Runtime.Resource {
	public class ResourceManager : AureFrameworkManager {
		private IResourceModule resourceModule;

		protected override void Awake() {
			base.Awake();

			resourceModule = GameMain.GetModule<ResourceModule>();
			if (resourceModule == null) {
				Debug.LogError("ResourceManager : ResourceModule is invalid");
				return;
			}
		}

		public T LoadAssetSync<T>(string assetName) where T : Object {
			return resourceModule.LoadAssetSync<T>(assetName);
		}

		public void LoadAssetAsync<T>(string assetName, Action<T> callBack) where T : Object {
			resourceModule.LoadAssetAsync<T>(assetName, callBack);
		}
	}
}