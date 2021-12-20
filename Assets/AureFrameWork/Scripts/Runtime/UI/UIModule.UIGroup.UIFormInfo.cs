//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework.ReferencePool;

namespace AureFramework.UI {
	public sealed partial class UIModule : AureFrameworkModule, IUIModule {
		private sealed partial class UIGroup {
			/// <summary>
			/// UI信息
			/// </summary>
			private sealed class UIFormInfo : IReference {
				private UIFormBase m_UIFormBase;
				private string uiName;
				private bool isPause;
				private int depth;
				
				public UIFormBase UIFormBase
				{
					get
					{
						return m_UIFormBase;
					}
				}

				public string UIName
				{
					get
					{
						return uiName;
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

				public static UIFormInfo Create(UIFormBase uiFormBase, string uiName)
				{
					var uiFormInfo = Aure.GetModule<IReferencePoolModule>().Acquire<UIFormInfo>();
					uiFormInfo.m_UIFormBase = uiFormBase;
					uiFormInfo.uiName = uiName;
					uiFormInfo.isPause = false;
					uiFormInfo.depth = 0;
					return uiFormInfo;
				}

				public void Clear()
				{
					m_UIFormBase = null;
					uiName = null;
					isPause = false;
					depth = 0;
				}
			}
		}
	}
}