//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework.Resource;
using UnityEngine;

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
	}
}