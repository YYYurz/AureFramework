//------------------------------------------------------------
// AureFramework
// Developed By YYYurz
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System.Net;

namespace AureFramework.Network
{
	public interface INetworkChannel
	{
		/// <summary>
		/// 获取网络频道名称
		/// </summary>
		string Name
		{
			get;
		}

		INetworkHelper NetworkHelper
		{
			get;
		}

		/// <summary>
		/// 连接服务器
		/// </summary>
		/// <param name="ipAddress"> IP地址 </param>
		/// <param name="port"> 端口号 </param>
		void Connect(IPAddress ipAddress, int port);

		/// <summary>
		/// 关闭网络跑到频道
		/// </summary>
		void CloseConnect();

		/// <summary>
		/// 发送消息包
		/// </summary>
		/// <param name="packet"> 消息包 </param>
		void Send(Packet packet);
	}
}