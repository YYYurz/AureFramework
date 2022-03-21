//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework.Event;
using AureFramework.ReferencePool;

namespace AureFramework.Network
{
	public class NetworkConnectedEventArgs : AureEventArgs
	{

		public static NetworkConnectedEventArgs Create()
		{
			var networkConnectedEventArgs = Aure.GetModule<IReferencePoolModule>().Acquire<NetworkConnectedEventArgs>();

			return networkConnectedEventArgs;
		}
		
		public override void Clear()
		{
			
		}
	}
}