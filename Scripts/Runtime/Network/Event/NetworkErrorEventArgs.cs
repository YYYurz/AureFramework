//------------------------------------------------------------
// AureFramework
// Developed By YYYurz
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework.Event;
using AureFramework.ReferencePool;

namespace AureFramework.Network
{
	public class NetworkErrorEventArgs : AureEventArgs
	{
		/// <summary>
		/// 错误信息
		/// </summary>
		public string ErrorMessage
		{
			get;
			private set;
		}

		public static NetworkErrorEventArgs Create(string errorMessage)
		{
			var networkErrorEventArgs = Aure.GetModule<IReferencePoolModule>().Acquire<NetworkErrorEventArgs>();
			networkErrorEventArgs.ErrorMessage = errorMessage;

			return networkErrorEventArgs;
		}
		
		public override void Clear()
		{
			ErrorMessage = null;
		}
	}
}