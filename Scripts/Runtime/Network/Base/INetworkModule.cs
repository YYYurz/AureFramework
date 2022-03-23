//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

namespace AureFramework.Network
{
	public interface INetworkModule
	{
		/// <summary>
		/// 获取网络频道
		/// </summary>
		/// <param name="channelName"> 网络频道名称 </param>
		/// <returns></returns>
		INetworkChannel GetNetworkChannel(string channelName);
		
		/// <summary>
		/// 创建网络频道
		/// </summary>
		/// <param name="channelName"> 网络频道名称 </param>
		/// <param name="networkHelper"> 网络频道辅助器 </param>
		/// <returns></returns>
		INetworkChannel CreateNetworkChannel(string channelName, INetworkHelper networkHelper);

		/// <summary>
		/// 销毁网络频道
		/// </summary>
		/// <param name="channelName"> 网络频道名称 </param>
		public void DestroyNetworkChannel(string channelName);
	}
}