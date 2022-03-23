//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System.IO;

namespace AureFramework.Network
{
	public interface INetworkHelper
	{
		/// <summary>
		/// 获取消息包头长度
		/// </summary>
		int PacketHeaderLength
		{
			get;
		}

		/// <summary>
		/// 关闭并清理网络频道辅助器
		/// </summary>
		void Shutdown();

		/// <summary>
		/// 发送心跳消息包
		/// </summary>
		/// <returns></returns>
		bool SendHeartBeat();

		/// <summary>
		/// 序列化消息包
		/// </summary>
		/// <param name="packet"> 要序列化的消息包 </param>
		/// <param name="destination"> 要序列化的目标流 </param>
		/// <returns></returns>
		bool Serialize(Packet packet, Stream destination);

		/// <summary>
		/// 反序列化消息包头
		/// </summary>
		/// <param name="source"> 要反序列化的来源流 </param>
		/// <returns></returns>
		IPacketHeader DeserializePacketHeader(Stream source);

		/// <summary>
		/// 反序列化消息包
		/// </summary>
		/// <param name="packetHeader"> 消息包头 </param>
		/// <param name="source"> 要反序列化的来源流 </param>
		/// <returns></returns>
		Packet DeserializePacket(IPacketHeader packetHeader, Stream source);
	}
}