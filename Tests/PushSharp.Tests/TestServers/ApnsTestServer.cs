using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace PushSharp.Tests.TestServers
{
	public delegate void NotificationReceivedDelegate(bool success, int identifier, string deviceToken, string payload);

	public class AppleTestServer : IDisposable
	{
		public AppleTestServer()
		{
			ResponseFilters = new List<ApnsResponseFilter>();
		}


		private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
		
		public List<ApnsResponseFilter> ResponseFilters { get; set; }
		private Socket listener;

		private ManualResetEvent waitFinished = new ManualResetEvent(false);
		Socket handler = null;

		public void Start(int port, int messageSize, NotificationReceivedDelegate received)
		{
			// Establish the local endpoint for the socket.
			// Dns.GetHostName returns the name of the 
			// host running the application.
			var localEndPoint = new IPEndPoint(IPAddress.Any, port);

			// Create a TCP/IP socket.
			listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

			// Bind the socket to the local endpoint and 
			// listen for incoming connections.
			listener.Bind(localEndPoint);
			listener.Listen(10);



			try
			{

			
				// Start listening for connections.
				while (!cancellationTokenSource.IsCancellationRequested)
				{
					// Program is suspended while waiting for an incoming connection.
					handler = listener.Accept();
					handler.ReceiveTimeout = 3000;
					handler.ReceiveBufferSize = messageSize;

					// An incoming connection needs to be processed.
					while (!cancellationTokenSource.IsCancellationRequested)
					{
						var bytes = new byte[messageSize];
						var bytesRec = handler.Receive(bytes, 0, bytes.Length, SocketFlags.None);

						if (bytesRec <= 0)
							break;

						var recdBuffer = bytes.ToList();

						Int16 msgLength = 0;

						//See if we have the msg length
						if (recdBuffer.Count >= 45)
					
						{
							var lenBytes = recdBuffer.GetRange(43, 2).ToArray();

							var payloadLen = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(lenBytes.ToArray(), 0));

							msgLength = (Int16)((Int16)payloadLen + (Int16)45);

							//See if we have the full msg
							if (recdBuffer.Count >= msgLength)
							{

								var payload = System.Text.Encoding.UTF8.GetString(recdBuffer.GetRange(45, payloadLen).ToArray());

								var identifierBytes = recdBuffer.GetRange(1, 4);
								var tokenBytes = recdBuffer.GetRange(11, 32);

								var identifier = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(identifierBytes.ToArray(), 0));
								var token = BitConverter.ToString(tokenBytes.ToArray()).Replace("-", "");

							
								recdBuffer.RemoveRange(0, msgLength);

								//	System.Threading.Thread.Sleep(rnd.Next(10, 100));

								bool hadError = false;

								foreach (var f in ResponseFilters)
								{
									if (f.IsMatch(identifier, token, payload))
									{
										var b2 = new byte[6];
										b2[0] = 0x01;
										b2[1] = BitConverter.GetBytes((short)f.Status)[0];
									
										Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(identifier)), 0, b2, 2, 4);

										//stream.Write(b2, 0, b2.Length);
										hadError = true;
										handler.Send(b2);

										received(false, identifier, token, payload);

										break;
									}
								}

								if (hadError)
									break;

								//Process the full msg
								received(true, identifier, token, payload);
							}

						}
						else
						{
							var b2 = new byte[6];
							b2[0] = 0x01;
							b2[1] = BitConverter.GetBytes((short)ApnsResponseStatus.InvalidPayloadSize)[0];
							handler.Send(b2);
							break;
						}
					}

					try { handler.Shutdown(SocketShutdown.Both); }  
					catch { }

					try { handler.Close(); } 
					catch { }
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}

			waitFinished.Set();

		}

		public void Dispose()
		{
			cancellationTokenSource.Cancel();

			if (handler != null)
			{
				//try { handler.Shutdown(SocketShutdown.Both); }
				//catch { }

				//try { handler.Close(); }
				//catch { }
			}

			Console.WriteLine("ApnsTestServer->Waiting...");
			waitFinished.WaitOne();
			Console.WriteLine("ApnsTestServer-> DISPOSE.");
		}

		
	}

	public class ApnsResponseFilter
	{
		public Func<int, string, string, bool> IsMatch { get; set; }

		public ApnsResponseStatus Status { get; set; }
	}

	public enum ApnsResponseStatus
	{
		Ok = 0,
		ProcessingError = 1,
		MissingDeviceToken = 2,
		MissingTopic = 3,
		MissingPayload = 4,
		InvalidTokenSize = 5,
		InvalidTopicSize = 6,
		InvalidPayloadSize = 7,
		InvalidToken = 8,
		Unknown = 255
	}
}
