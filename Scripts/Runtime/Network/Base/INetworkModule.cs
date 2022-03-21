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
		/// 创建网络频道
		/// </summary>
		/// <param name="channelName"></param>
		/// <param name="networkHelper"></param>
		/// <returns></returns>
		INetworkChannel CreateNetworkChannel(string channelName, INetworkHelper networkHelper = null);

		public void DestroyNetworkChannel(string channelName);
	}
}