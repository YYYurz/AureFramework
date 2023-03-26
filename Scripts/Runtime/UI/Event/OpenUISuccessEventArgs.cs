//------------------------------------------------------------
// AureFramework
// Developed By YYYurz
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework.Event;
using AureFramework.ReferencePool;

namespace AureFramework.UI
{
	/// <summary>
	/// 打开UI成功事件
	/// </summary>
	public class OpenUISuccessEventArgs : AureEventArgs
	{
		/// <summary>
		/// UIForm
		/// </summary>
		public UIFormBase UIForm
		{
			get;
			private set;
		}
		
		public static OpenUISuccessEventArgs Create(UIFormBase uiForm)
		{
			var openUISuccessEventArgs = Aure.GetModule<IReferencePoolModule>().Acquire<OpenUISuccessEventArgs>();
			openUISuccessEventArgs.UIForm = uiForm;

			return openUISuccessEventArgs;
		}
		
		public override void Clear()
		{
			UIForm = null;
		}
	}
}