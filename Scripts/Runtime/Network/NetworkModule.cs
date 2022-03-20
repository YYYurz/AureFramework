//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace AureFramework.Network
{
	public sealed partial class NetworkModule : AureFrameworkModule, INetworkModule
	{
		private readonly Dictionary<string, NetworkChannel> channelDic = new Dictionary<string, NetworkChannel>();
		
		/// <summary>
		/// 模块初始化，只在第一次被获取时调用一次
		/// </summary>
		public override void Init()
		{
			
		}

		/// <summary>
		/// 框架轮询
		/// </summary>
		/// <param name="elapseTime"> 距离上一帧的流逝时间，秒单位 </param>
		/// <param name="realElapseTime"> 距离上一帧的真实流逝时间，秒单位 </param>
		public override void Tick(float elapseTime, float realElapseTime)
		{
			
		}

		/// <summary>
		/// 框架清理
		/// </summary>
		public override void Clear()
		{
			foreach (var channel in channelDic)
			{
				
			}
			
			channelDic.Clear();
		}

		/// <summary>
		/// 创建网络频道
		/// </summary>
		/// <param name="channelName"></param>
		/// <returns></returns>
		public INetworkChannel CreateNetworkChannel(string channelName)
		{
			if (string.IsNullOrEmpty(channelName))
			{
				Debug.LogError("NetworkModule : Network channel name is invalid.");
				return null;
			}

			if (channelDic.ContainsKey(channelName))
			{
				Debug.LogError($"NetworkModule : Network channel is already exist, name :{channelName}.");
				return null;
			}
			
			var networkChannel = new NetworkChannel();
			channelDic.Add(channelName, networkChannel);

			return networkChannel;
		}
	}
}