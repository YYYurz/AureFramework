//------------------------------------------------------------
// AureFramework
// Developed By YYYurz
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework.ObjectPool;
using AureFramework.ReferencePool;
using AureFramework.Resource;
using UnityEngine;

namespace AureFramework.UI
{
	public sealed partial class UIModule : AureFrameworkModule, IUIModule
	{
		/// <summary>
		/// UI对象池对象
		/// </summary>
		private class UIObject : ObjectBase
		{
			/// <summary>
			/// UI游戏物体
			/// </summary>
			public GameObject UIGameObject
			{
				get;
				private set;
			}

			public static UIObject Create(string uiName, GameObject uiGameObject)
			{
				var uiObject = Aure.GetModule<IReferencePoolModule>().Acquire<UIObject>();
				uiObject.Name = uiName;
				uiObject.UIGameObject = uiGameObject;

				return uiObject;
			}

			public override void OnRelease()
			{
				base.OnRelease();

				Aure.GetModule<IResourceModule>().ReleaseAsset(UIGameObject);
			}

			public override void Clear()
			{
				base.Clear();

				UIGameObject = null;
			}
		}
	}
}