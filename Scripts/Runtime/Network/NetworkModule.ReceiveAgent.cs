//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using System.IO;
using System.Net.Sockets;
using UnityEngine;

namespace AureFramework.Network
{
	public sealed partial class NetworkModule : AureFrameworkModule, INetworkModule
	{
		private class ReceiveAgent : IDisposable
		{
			private const int DefaultBufferLength = 1024 * 64;
			private readonly NetworkChannel channel;
			private MemoryStream memoryStream;
			private bool disposed;
			
			public ReceiveAgent(NetworkChannel channel)
			{
				this.channel = channel;
				memoryStream = new MemoryStream(DefaultBufferLength);
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
			
			public void Dispose()
			{
				InternalDispose();
				GC.SuppressFinalize(this);
			}
			
			private void InternalDispose()
			{
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
					Debug.LogError(exception is SocketException socketException ? socketException.SocketErrorCode.ToString() : exception.Message);
				}
			}

			private void OnReceiveCallback(IAsyncResult result)
			{
				var internalSocket = (Socket)result.AsyncState;
				if (!channel.Socket.Connected)
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
					Debug.LogError(exception is SocketException socketException ? socketException.SocketErrorCode.ToString() : exception.Message);

					return;
				}

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

				// var processSuccess = false;
				// if (m_ReceiveState.PacketHeader != null)
				// {
				// 	processSuccess = ProcessPacket();
				// 	m_ReceivedPacketCount++;
				// }
				// else
				// {
				// 	processSuccess = ProcessPacketHeader();
				// }
				//
				// if (processSuccess)
				// {
				// 	Receive();
				// }
			}
		}
	}
}