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
			foreach (var channel in channelDic)
			{
				channel.Value.Update();
			}
		}

		/// <summary>
		/// 框架清理
		/// </summary>
		public override void Clear()
		{
			foreach (var channel in channelDic)
			{
				channel.Value.ShutDown();
			}
			
			channelDic.Clear();
		}
		
		/// <summary>
		/// 获取网络频道
		/// </summary>
		/// <param name="channelName"> 网络频道名称 </param>
		/// <returns></returns>
		public INetworkChannel GetNetworkChannel(string channelName)
		{
			if (string.IsNullOrEmpty(channelName))
			{
				Debug.LogError("NetworkModule : Network channel name is invalid.");
				return null;
			}
			
			if (channelDic.TryGetValue(channelName, out var networkChannel))
			{
				return networkChannel;
			}

			return null;
		}

		/// <summary>
		/// 创建网络频道
		/// </summary>
		/// <param name="channelName"> 网络频道名称 </param>
		/// <param name="networkHelper"> 网络频道辅助器 </param>
		/// <returns></returns>
		public INetworkChannel CreateNetworkChannel(string channelName, INetworkHelper networkHelper)
		{
			if (string.IsNullOrEmpty(channelName))
			{
				Debug.LogError("NetworkModule : Network channel name is invalid.");
				return null;
			}

			if (networkHelper == null)
			{
				Debug.LogError("NetworkModule : Network Helper is null.");
				return null;
			}

			if (channelDic.ContainsKey(channelName))
			{
				Debug.LogError($"NetworkModule : Network channel is already exist, name :{channelName}.");
				return null;
			}
			
			var networkChannel = new NetworkChannel(channelName, networkHelper);
			channelDic.Add(channelName, networkChannel);

			return networkChannel;
		}

		/// <summary>
		/// 销毁网络频道
		/// </summary>
		/// <param name="channelName"> 网络频道名称 </param>
		public void DestroyNetworkChannel(string channelName)
		{
			if (string.IsNullOrEmpty(channelName))
			{
				Debug.LogError("NetworkModule : Network channel name is invalid.");
				return;	
			}
			
			if (channelDic.TryGetValue(channelName, out var channel))
			{
				channel.ShutDown();
				channelDic.Remove(channelName);
			}
		}
	}
}