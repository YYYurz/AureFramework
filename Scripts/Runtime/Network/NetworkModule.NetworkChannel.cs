//------------------------------------------------------------
// AureFramework
// Developed By YYYurz
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using System.Net;
using System.Net.Sockets;
using AureFramework.Event;
using UnityEngine;

namespace AureFramework.Network
{
	public sealed partial class NetworkModule : AureFrameworkModule, INetworkModule
	{
		private class NetworkChannel : INetworkChannel
		{
			private readonly SendAgent sendAgent;
			private readonly ReceiveAgent receiveAgent;
			private readonly INetworkHelper networkHelper;
			private readonly IEventModule eventModule;
			private readonly string name;
			private bool active;
			private Socket socket;

			public NetworkChannel(string name, INetworkHelper networkHelper)
			{
				this.name = name;
				this.networkHelper = networkHelper;
				sendAgent = new SendAgent(this);
				receiveAgent = new ReceiveAgent(this);
				eventModule = Aure.GetModule<IEventModule>();
			}

			public string Name
			{
				get
				{
					return name;
				}
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
			public void Update()
			{
				if (!active || socket == null)
				{
					return;
				}
				
				sendAgent.Update();
				receiveAgent.Update();
			}

			public void ShutDown()
			{
				CloseConnect();
				networkHelper.Shutdown();
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
						Debug.LogError("NetworkModule : Not supported address family.");
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
					eventModule.Fire(this, NetworkErrorEventArgs.Create(exception.Message));
					throw;
				}
				
				sendAgent.Reset();
				receiveAgent.Reset();
			}

			public void CloseConnect()
			{
				lock (this)
				{
					if (socket == null || !socket.Connected)
					{
						return;
					}
					
					try
					{
						socket.Shutdown(SocketShutdown.Both);
					}
					catch(Exception exception)
					{
						eventModule.Fire(this, NetworkErrorEventArgs.Create(exception.Message));
						throw;
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
					eventModule.Fire(this, NetworkErrorEventArgs.Create(exception.Message));
					throw;
				}

				active = true;
				receiveAgent.Receive();
				eventModule.Fire(this, NetworkConnectedEventArgs.Create());
			}
		}
	}
}