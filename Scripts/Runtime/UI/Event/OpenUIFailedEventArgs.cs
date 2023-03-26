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
	/// 打开UI失败事件
	/// </summary>
	public class OpenUIFailedEventArgs : AureEventArgs
	{
		/// <summary>
		/// UI名称
		/// </summary>
		public string UIName
		{
			get;
			private set;
		}

		/// <summary>
		/// 失败信息
		/// </summary>
		public string FailedMessage
		{
			get;
			private set;
		}
		
		public static OpenUIFailedEventArgs Create(string uiName, string failedMessage)
		{
			var openUIFailedEventArgs = Aure.GetModule<IReferencePoolModule>().Acquire<OpenUIFailedEventArgs>();
			openUIFailedEventArgs.UIName = uiName;
			openUIFailedEventArgs.FailedMessage = failedMessage;

			return openUIFailedEventArgs;
		}
		
		public override void Clear()
		{
			UIName = null;
			FailedMessage = null;
		}
	}
}