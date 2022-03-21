//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System.Net;
using System.Net.Sockets;

namespace AureFramework.Network
{
	public interface INetworkChannel
	{
		string Name
		{
			get;
		}

		Socket Socket
		{
			get;
		}

		INetworkHelper NetworkHelper
		{
			get;
		}

		void Connect(IPAddress ipAddress, int port);

		void CloseConnect();

		void Send(Packet packet);
	}
}