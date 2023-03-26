//------------------------------------------------------------
// AureFramework
// Developed By YYYurz
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using AureFramework;
using AureFramework.Event;
using AureFramework.Network;
using AureFramework.ReferencePool;

namespace AureFramework.Network
{
	public class NetworkReceiveEventArgs : AureEventArgs
	{
		public Packet Packet
		{
			get;
			private set;
		}

		public static NetworkReceiveEventArgs Create(Packet packet)
		{
			var networkReceiveEventArgs = Aure.GetModule<IReferencePoolModule>().Acquire<NetworkReceiveEventArgs>();
			networkReceiveEventArgs.Packet = packet;

			return networkReceiveEventArgs;
		} 
		
		public override void Clear()
		{
			Packet = null;
		}
	}
}