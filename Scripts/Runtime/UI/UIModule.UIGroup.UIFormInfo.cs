//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework.ReferencePool;

namespace AureFramework.UI
{
	public sealed partial class UIModule : AureFrameworkModule, IUIModule
	{
		private sealed partial class UIGroup
		{
			/// <summary>
			/// UI信息
			/// </summary>
			private sealed class UIFormInfo : IReference
			{
				private UIFormBase uiFormBase;
				private UIObject uiObject;
				private string uiAssetName;
				private bool isPause;
				private int depth;

				public UIFormBase UIFormBase
				{
					get
					{
						return uiFormBase;
					}
				}

				public UIObject UIObject
				{
					get
					{
						return uiObject;
					}
					set
					{
						uiObject = value;
					}
				}

				public string UIName
				{
					get
					{
						return uiFormBase.UIName;
					}
					set
					{
						uiFormBase.UIName = value;
					}
				}

				public string AssetName
				{
					get
					{
						return uiAssetName;
					}
					set
					{
						uiAssetName = value;
					}
				}

				public bool IsPause
				{
					get
					{
						return isPause;
					}
					set
					{
						isPause = value;
					}
				}

				public int Depth
				{
					get
					{
						return depth;
					}
					set
					{
						depth = value;
					}
				}

				public static UIFormInfo Create(UIFormBase uiFormBase, UIObject uiObject, string uiName, string assetName)
				{
					var uiFormInfo = Aure.GetModule<IReferencePoolModule>().Acquire<UIFormInfo>();
					uiFormInfo.uiFormBase = uiFormBase;
					uiFormInfo.uiObject = uiObject;
					uiFormInfo.uiFormBase.UIName = uiName;
					uiFormInfo.uiAssetName = assetName;
					uiFormInfo.isPause = false;
					uiFormInfo.depth = 0;
					return uiFormInfo;
				}

				public void Clear()
				{
					uiObject = null;
					uiFormBase.UIName = null;
					uiFormBase = null;
					isPause = false;
					depth = 0;
				}
			}
		}
	}
}