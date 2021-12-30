//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

namespace AureFramework.UI
{
	public sealed partial class UIModule : AureFrameworkModule, IUIModule
	{
		private sealed partial class UIGroup
		{
			/// <summary>
			/// UI处理任务类型
			/// </summary>
			private enum UITaskType
			{
				None,

				/// <summary>
				/// 打开UI
				/// </summary>
				OpenUI,

				/// <summary>
				/// 关闭UI
				/// </summary>
				CloseUI,

				/// <summary>
				/// 完成
				/// </summary>
				Complete,
			}
		}
	}
}