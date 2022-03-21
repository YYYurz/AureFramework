//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using UnityEngine;

namespace AureFramework.Network
{
	public sealed partial class NetworkModule : AureFrameworkModule, INetworkModule
	{
		private class SendAgent
		{
			private const int DefaultBufferLength = 1024 * 64;
			private readonly Queue<Packet> sendPacketQueue;
			private readonly NetworkChannel channel;
			private MemoryStream memoryStream;
			private bool disposed;

			public SendAgent(NetworkChannel channel)
			{
				this.channel = channel;
				sendPacketQueue = new Queue<Packet>();
				memoryStream = new MemoryStream(DefaultBufferLength);
				disposed = false;
			}

			public void Update()
			{
				ProcessSend();
			}

			public void Reset()
			{
				memoryStream.Position = 0L;
				memoryStream.SetLength(0L);
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
			
			public void Send(Packet packet)
			{
				lock (sendPacketQueue)
				{
					sendPacketQueue.Enqueue(packet);
				}
			}

			private void ProcessSend()
			{
				lock (sendPacketQueue)
				{
					if (memoryStream.Length > 0 || sendPacketQueue.Count <= 0)
					{
						return;
					}
					
					while (sendPacketQueue.Count > 0)
					{
						var	packet = sendPacketQueue.Dequeue();
						bool serializeResult;
						try
						{
							serializeResult = channel.NetworkHelper.Serialize(packet, memoryStream);
						}
						catch (Exception exception)
						{
							channel.Active = false;
							Debug.LogError(exception is SocketException socketException ? socketException.SocketErrorCode.ToString() : exception.Message);
							return;
						}

						if (!serializeResult)
						{
							Debug.LogError("NetworkModule : Serialized packet failed.");
							return;
						}
					}

					memoryStream.Position = 0L;
					SendAsync();
				}
			}

			private void SendAsync()
			{
				try
				{
					channel.Socket.BeginSend(memoryStream.GetBuffer(), (int)memoryStream.Position, (int)(memoryStream.Length - memoryStream.Position), SocketFlags.None, OnSendCallback, channel.Socket);
				}
				catch (Exception exception)
				{
					channel.Active = false;
					Debug.LogError(exception is SocketException socketException ? socketException.SocketErrorCode.ToString() : exception.Message);
				}
			}

			private void OnSendCallback(IAsyncResult result)
			{
				var socket = (Socket) result.AsyncState;
				if (!socket.Connected)
				{
					return;
				}

				int bytesSent;
				try
				{
					bytesSent = socket.EndSend(result);
				}
				catch (Exception exception)
				{
					channel.Active = false;
					Debug.LogError(exception is SocketException socketException ? socketException.SocketErrorCode.ToString() : exception.Message);
					return;
				}

				memoryStream.Position += bytesSent;
				if (memoryStream.Position < memoryStream.Length)
				{
					SendAsync();
					return;
				}

				Reset();
			}
		}
	}
}