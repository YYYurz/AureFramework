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
			/// UI处理任务
			/// </summary>
			private sealed class UITask : IReference {
				public string UIName
				{
					private set;
					get;
				}

				public UITaskType UITaskType
				{
					private set;
					get;
				}

				public static UITask Create(string uiName, UITaskType uiTaskType) {
					var uiTask = Aure.GetModule<IReferencePoolModule>().Acquire<UITask>();
					uiTask.UIName = uiName;
					uiTask.UITaskType = uiTaskType;
					return uiTask;
				}
				
				public void Clear() {
					UIName = null;
					UITaskType = UITaskType.Discard;
				}
			}
		}
	}
}