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
	public delegate void ApnsNotificationReceivedDelegate(bool success, int identifier, string deviceToken, string payload);

	// State object for reading client data asynchronously
	public class StateObject
	{
		public StateObject(int bufferSize)
		{
			this.buffer = new byte[bufferSize];
		}


		// Client  socket.
		public Socket workSocket = null;
		
		// Receive buffer.
		public byte[] buffer;
		// Received data string.
		public StringBuilder sb = new StringBuilder();

        public List<byte> recdBuffer = new List<byte>();
	}

	public class ApnsTestServer
	{
		public List<ApnsResponseFilter> ResponseFilters { get; set; }

		public ApnsNotificationReceivedDelegate Received { get; set; }
		public int Port { get; set; }
		public int MessageSize { get; set; }

		public ApnsTestServer()
		{
			ResponseFilters = new List<ApnsResponseFilter>();
		}

		// Thread signal.
		public ManualResetEvent allDone = new ManualResetEvent(false);
		public ManualResetEvent waitServer = new ManualResetEvent(false);
		private Socket listener;
		CancellationTokenSource cancelServerTokenSource = new CancellationTokenSource();

		public void Start(int port, int messageSize, ApnsNotificationReceivedDelegate received)
		{
			this.Port = port;
			this.MessageSize = messageSize;
			this.Received = received;

			// Data buffer for incoming data.
			//byte[] bytes = new Byte[1024];

			// Establish the local endpoint for the socket.
			// The DNS name of the computer
			// running the listener is "host.contoso.com".
			
			var localEndPoint = new IPEndPoint(IPAddress.Any, port);

			// Create a TCP/IP socket.
			listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

			// Bind the socket to the local endpoint and listen for incoming connections.
			try
			{
				listener.Bind(localEndPoint);
				listener.Listen(100);

				
				while (!cancelServerTokenSource.IsCancellationRequested)
				{
					// Set the event to nonsignaled state.
					allDone.Reset();

					try
					{
						// Start an asynchronous socket to listen for connections.
						//Console.WriteLine("Waiting for a connection...");
						listener.BeginAccept(
							new AsyncCallback(AcceptCallback),
							listener);

					}
					catch (Exception ex)
					{
						Console.WriteLine("Listener Failed: " + ex);
						
					}

					allDone.WaitOne();

					// Wait until a connection is made before continuing.
					//while (!allDone.WaitOne(1000))
					//{
					//if (cancelServerTokenSource.IsCancellationRequested)
					//break;
					//}
				}

				waitServer.Set();

			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}

			Console.WriteLine("Wrapping up listener...");
		}

		public void AcceptCallback(IAsyncResult ar)
		{
			// Signal the main thread to continue.
			allDone.Set();

			try
			{
				// Get the socket that handles the client request.
				Socket listener = (Socket)ar.AsyncState;
				Socket handler = listener.EndAccept(ar);

				// Create the state object.
				StateObject state = new StateObject(this.MessageSize);
				state.workSocket = handler;
				handler.BeginReceive(state.buffer, 0, this.MessageSize, 0, new AsyncCallback(ReadCallback), state);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Accept Failed: " + ex);
			}

		}

        
 
		public void ReadCallback(IAsyncResult ar)
		{
		    try
		    {


		        // Retrieve the state object and the handler socket
		        // from the asynchronous state object.
		        StateObject state = (StateObject) ar.AsyncState;
		        Socket handler = state.workSocket;

		        // Read data from the client socket. 
		        int bytesRead = handler.EndReceive(ar);

		        bool hadError = false;

		        var payload = string.Empty;
		        var identifier = 0;
		        var token = string.Empty;
		        var errorResponseData = new byte[6];

		        var localBuffer = new byte[bytesRead];

		        Array.Copy(state.buffer, localBuffer, bytesRead);

		        state.recdBuffer.AddRange(localBuffer);

		        if (state.recdBuffer.Count < MessageSize)
		        {
		            //Console.WriteLine("Non-Full Buffer: " + state.recdBuffer.Count);
		            handler.BeginReceive(state.buffer, 0, this.MessageSize, 0, new AsyncCallback(ReadCallback), state);
		            return;
		        }


		        var lenBytes = state.recdBuffer.GetRange(43, 2).ToArray();

		        var payloadLen = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(lenBytes.ToArray(), 0));

		        var msgLength = (Int16) ((Int16) payloadLen + (Int16) 45);



		        payload = System.Text.Encoding.UTF8.GetString(state.recdBuffer.GetRange(45, payloadLen).ToArray());

		        var identifierBytes = state.recdBuffer.GetRange(1, 4);
		        var tokenBytes = state.recdBuffer.GetRange(11, 32);

		        identifier = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(identifierBytes.ToArray(), 0));
		        token = BitConverter.ToString(tokenBytes.ToArray()).Replace("-", "");


		        //Remove our message from the buffer
		        state.recdBuffer.RemoveRange(0, MessageSize);

		        //Console.WriteLine("Buffer Length: " + state.recdBuffer.Count);

		        foreach (var f in ResponseFilters)
		        {
		            if (f.IsMatch(identifier, token, payload))
		            {
		                errorResponseData[0] = 0x01;
		                errorResponseData[1] = BitConverter.GetBytes((short) f.Status)[0];

		                Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(identifier)), 0, errorResponseData,
		                                 2, 4);

		                //stream.Write(b2, 0, b2.Length);
		                hadError = true;
		                break;
		            }
		        }

		        if (hadError)
		        {
		            //Console.WriteLine("Server Recd Notification with Error");
		            var rnd = new Random();
		            System.Threading.Thread.Sleep(rnd.Next(10, 150));

		            state.recdBuffer.Clear();

		            Received(false, identifier, token, payload);
		            Send(handler, errorResponseData);
		        }
		        else
		        {
		            Received(true, identifier, token, payload);
		            handler.BeginReceive(state.buffer, 0, this.MessageSize, 0, new AsyncCallback(ReadCallback), state);
		        }
		    }
		    catch (Exception ex)
		    {
		        Console.WriteLine("EndReceive Fail: " + ex);
		    }
		}

		private void Send(Socket handler, byte[] byteData)
		{
			// Convert the string data to byte data using ASCII encoding.


			// Begin sending the data to the remote device.
			handler.BeginSend(byteData, 0, byteData.Length, 0,
			                  new AsyncCallback(SendCallback), handler);
		}

		private void SendCallback(IAsyncResult ar)
		{
			Socket handler = null;

			try
			{
				// Retrieve the socket from the state object.
				handler = (Socket) ar.AsyncState;

				// Complete sending the data to the remote device.
				int bytesSent = handler.EndSend(ar);
				//Console.WriteLine("Sent {0} bytes to client.", bytesSent);

			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}
			finally
			{
				if (handler != null)
				{
					try { handler.Shutdown(SocketShutdown.Both); } catch { }

					try { handler.Close(); } catch { }
				}
			}



		}

		public void Dispose()
		{
			
			cancelServerTokenSource.Cancel();

			try { listener.Shutdown(SocketShutdown.Both); }
			catch { }

			try { listener.Close(); }
			catch { }

			waitServer.WaitOne();
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
