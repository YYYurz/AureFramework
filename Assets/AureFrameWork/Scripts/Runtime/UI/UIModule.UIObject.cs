//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework.ObjectPool;
using AureFramework.ReferencePool;
using AureFramework.Resource;
using UnityEngine;

namespace AureFramework.UI {
	public sealed partial class UIModule : AureFrameworkModule, IUIModule {
		private class UIObject : ObjectBase {
			/// <summary>
			/// UI游戏内物体
			/// </summary>
			public GameObject UIGameObject
			{
				get;
				private set;
			}

			public static UIObject Create(string uiName, GameObject uiGameObject) {
				var uiObject = Aure.GetModule<IReferencePoolModule>().Acquire<UIObject>();
				uiObject.Name = uiName;
				uiObject.UIGameObject = uiGameObject;

				return uiObject;
			}

			public override void OnRelease() {
				base.OnRelease();
				
				Debug.Log("OnRelease");
				Aure.GetModule<IResourceModule>().ReleaseAsset(UIGameObject);
			}
		}
	}
}