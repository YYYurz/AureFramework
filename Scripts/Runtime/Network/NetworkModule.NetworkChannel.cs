//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace AureFramework.Network
{
	public sealed partial class NetworkModule : AureFrameworkModule, INetworkModule
	{
		private class NetworkChannel : INetworkChannel
		{
			private string name;
			private Socket socket;
			private readonly SendAgent sendAgent;
			private readonly ReceiveAgent receiveAgent;
			private readonly INetworkHelper networkHelper;
			private bool active;

			public NetworkChannel(string name, INetworkHelper networkHelper)
			{
				this.name = name;
				this.networkHelper = networkHelper;
				sendAgent = new SendAgent(this);
				receiveAgent = new ReceiveAgent(this);
			}

			public Socket Socket
			{
				get
				{
					return socket;
				}
			}

			public INetworkHelper NetworkHelper
			{
				get
				{
					return networkHelper;
				}
			}

			public bool Active
			{
				get
				{
					return active;
				}
				set
				{
					active = value;
				}
			}
			
			/// <summary>
			/// 轮询
			/// </summary>
			/// <param name="elapseTime"> 距离上一帧的流逝时间，秒单位 </param>
			/// <param name="realElapseTime"> 距离上一帧的真实流逝时间，秒单位 </param>
			public void Update(float elapseTime, float realElapseTime)
			{
				if (!active || socket == null)
				{
					return;
				}
				
				sendAgent.Update();
				receiveAgent.Update();
			}
			
			public void Connect(IPAddress ipAddress, int port)
			{
				if (socket != null)
				{
					CloseConnect();
				}

				switch (ipAddress.AddressFamily)
				{
					case AddressFamily.InterNetwork :
					case AddressFamily.InterNetworkV6 :
						break;
					default:
					{
						Debug.LogError($"Not supported address family.");
						return;
					}
				}
				
				socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
				
				try
				{
					socket.BeginConnect(ipAddress, port, OnConnectCallback, new ConnectInfo(socket));
				}
				catch (Exception exception)
				{
					Debug.LogError(exception is SocketException socketException ? socketException.SocketErrorCode.ToString() : exception.Message);
					return;
				}
				
				sendAgent.Reset();
			}

			public void CloseConnect()
			{
				lock (this)
				{
					if (socket == null)
					{
						return;
					}
					
					try
					{
						socket.Shutdown(SocketShutdown.Both);
					}
					catch(Exception e)
					{
						Debug.LogError($"NetworkModule : Close socket error, {e.Message}");
					}
					finally
					{
						socket.Close();
						socket = null;
					}
				}
			}

			public void Send(Packet packet)
			{
				if(packet == null)
				{
					Debug.LogError("NetworkModule : packet is invalid.");
					return;
				}

				if (socket == null)
				{
					Debug.LogError("NetworkModule : Socket is not connected.");
					return;
				}

				if (!active)
				{
					Debug.LogError("NetworkModule : Socket is not active.");
					return;
				}
				
				sendAgent.Send(packet);
			}

			private void OnConnectCallback(IAsyncResult result)
			{
				var socketUserData = (ConnectInfo)result.AsyncState;
				try
				{
					socketUserData.Socket.EndConnect(result);
				}
				catch (Exception exception)
				{
					active = false;
					if (exception is SocketException socketException)
					{
						Debug.LogError(socketException.SocketErrorCode);
					}
					else
					{
						Debug.LogError(exception.Message);
					}
					
					return;
				}

				active = true;
				receiveAgent.Receive();
			}
		}
	}
}