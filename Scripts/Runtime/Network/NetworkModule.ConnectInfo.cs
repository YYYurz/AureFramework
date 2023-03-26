//------------------------------------------------------------
// AureFramework
// Developed By YYYurz
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System.Net.Sockets;

namespace AureFramework.Network
{
	public sealed partial class NetworkModule : AureFrameworkModule, INetworkModule
	{
		/// <summary>
		/// 异步连接信息
		/// </summary>
		public class ConnectInfo
		{
			private readonly Socket socket;

			public ConnectInfo(Socket socket)
			{
				this.socket = socket;
			}
			
			public Socket Socket
			{
				get
				{
					return socket;
				}
			}
		}
	}
}