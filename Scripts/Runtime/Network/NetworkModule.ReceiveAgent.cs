//------------------------------------------------------------
// AureFramework
// Developed By YYYurz
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using System.IO;
using System.Net.Sockets;
using AureFramework.Event;

namespace AureFramework.Network
{
	public sealed partial class NetworkModule : AureFrameworkModule, INetworkModule
	{
		private class ReceiveAgent : IDisposable
		{
			private readonly NetworkChannel channel;
			private readonly IEventModule eventModule;
			private IPacketHeader packetHeader;
			private MemoryStream memoryStream;
			private bool disposed;
			
			public ReceiveAgent(NetworkChannel channel)
			{
				this.channel = channel;
				eventModule = Aure.GetModule<IEventModule>();
				memoryStream = new MemoryStream(1024 * 64);
				disposed = false;
			}

			public MemoryStream MemoryStream
			{
				get
				{
					return memoryStream;
				}
			}

			public void Update()
			{
				
			}

			public void Reset()
			{
				memoryStream.Position = 0L;
				memoryStream.SetLength(channel.NetworkHelper.PacketHeaderLength);
				packetHeader = null;
			}
			
			public void Dispose()
			{
				InternalDispose();
				GC.SuppressFinalize(this);
			}
			
			private void InternalDispose()
			{
				if (disposed)
				{
					return;
				}
			
				if (memoryStream != null)
				{
					memoryStream.Dispose();
					memoryStream = null;
				}

				disposed = true;
			}
			
			public void Receive()
			{
				try
				{
					channel.Socket.BeginReceive(memoryStream.GetBuffer(), (int)memoryStream.Position, (int)(memoryStream.Length - memoryStream.Position), SocketFlags.None, OnReceiveCallback, channel.Socket);
				}
				catch (Exception exception)
				{
					channel.Active = false;
					eventModule.Fire(this, NetworkErrorEventArgs.Create(exception.Message));
					throw;
				}
			}

			private void OnReceiveCallback(IAsyncResult result)
			{
				var internalSocket = (Socket) result.AsyncState;
				if (!internalSocket.Connected)
				{
					return;
				}

				var bytesReceived = 0;
				try
				{
					bytesReceived = internalSocket.EndReceive(result);
				}
				catch (Exception exception)
				{
					channel.Active = false;
					eventModule.Fire(this, NetworkErrorEventArgs.Create(exception.Message));
					throw;
				}

				// 服务器主动断开
				if (bytesReceived <= 0)
				{
					channel.CloseConnect();
					return;
				}

				memoryStream.Position += bytesReceived;
				if (memoryStream.Position < memoryStream.Length)
				{
					Receive();
					return;
				}

				memoryStream.Position = 0L;

				var processSuccess = packetHeader == null ? ProcessPacketHeader() : ProcessPacket();
				if (processSuccess)
				{
					Receive();
				}
			}

			private bool ProcessPacketHeader()
			{
				try
				{
					packetHeader = channel.NetworkHelper.DeserializePacketHeader(memoryStream);
					if (packetHeader == null)
					{
						eventModule.Fire(this, NetworkErrorEventArgs.Create("NetworkModule : Packet header is invalid."));
						return false;
					}
				
					memoryStream.Position = 0L;
					memoryStream.SetLength(packetHeader.PacketLength);
					
					if (packetHeader.PacketLength <= 0)
					{
						return ProcessPacket();
					}
				}
				catch (Exception exception)
				{
					channel.Active = false;
					eventModule.Fire(this, NetworkErrorEventArgs.Create(exception.Message));
					throw;
				}

				return true;
			}

			private bool ProcessPacket()
			{
				try
				{
					var packet = channel.NetworkHelper.DeserializePacket(packetHeader, memoryStream);
					if (packet != null)
					{
						eventModule.Fire(this, NetworkReceiveEventArgs.Create(packet));
					}

					Reset();
				}
				catch (Exception exception)
				{
					channel.Active = false;
					eventModule.Fire(this, NetworkErrorEventArgs.Create(exception.Message));
					return false;
				}

				return true;
			}
		}
	}
}