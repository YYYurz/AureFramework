//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

namespace AureFramework.Network
{
	public interface IPacketHeader
	{
		/// <summary>
		/// 获取网络消息包长度
		/// </summary>
		int PacketLength
		{
			get;
		}
	}
}