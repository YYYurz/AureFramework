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
		/// 初始化网络频道辅助器。
		/// </summary>
		/// <param name="networkChannel"> 网络频道 </param>
		void Initialize(INetworkChannel networkChannel);

		/// <summary>
		/// 关闭并清理网络频道辅助器
		/// </summary>
		void Shutdown();

		/// <summary>
		/// 准备进行连接
		/// </summary>
		void PrepareForConnecting();

		/// <summary>
		/// 发送心跳消息包
		/// </summary>
		/// <returns> 是否发送心跳消息包成功 </returns>
		bool SendHeartBeat();

		/// <summary>
		/// 序列化消息包
		/// </summary>
		/// <param name="packet"> 要序列化的消息包 </param>
		/// <param name="destination"> 要序列化的目标流 </param>
		/// <returns>是否序列化成功。</returns>
		bool Serialize(Packet packet, Stream destination);

		/// <summary>
		/// 反序列化消息包头。
		/// </summary>
		/// <param name="source">要反序列化的来源流。</param>
		/// <param name="customErrorData">用户自定义错误数据。</param>
		/// <returns>反序列化后的消息包头。</returns>
		// IPacketHeader DeserializePacketHeader(Stream source, out object customErrorData);

		/// <summary>
		/// 反序列化消息包。
		/// </summary>
		/// <param name="packetHeader">消息包头。</param>
		/// <param name="source">要反序列化的来源流。</param>
		/// <param name="customErrorData">用户自定义错误数据。</param>
		/// <returns>反序列化后的消息包。</returns>
		// Packet DeserializePacket(IPacketHeader packetHeader, Stream source, out object customErrorData);

	}
}