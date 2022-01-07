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
			/// 内部UI处理任务
			/// </summary>
			private sealed class UITask : IReference
			{
				public string UIName
				{
					private set;
					get;
				}

				public string UIAssetName
				{
					private set;
					get;
				}
				
				public object UserData
				{
					private set;
					get;
				}

				public UITaskType UITaskType
				{
					set;
					get;
				}

				public static UITask Create(string uiName, string uiAssetName, UITaskType uiTaskType, object userData)
				{
					var uiTask = Aure.GetModule<IReferencePoolModule>().Acquire<UITask>();
					uiTask.UIName = uiName;
					uiTask.UIAssetName = uiAssetName;
					uiTask.UserData = userData;
					uiTask.UITaskType = uiTaskType;
					return uiTask;
				}

				public void Clear()
				{
					UIName = null;
					UIAssetName = null;
					UserData = null;
					UITaskType = UITaskType.None;
				}
			}
		}
	}
}