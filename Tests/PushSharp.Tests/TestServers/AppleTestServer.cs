using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace PushSharp.Tests.TestServers
{
	public delegate void NotificationReceivedDelegate(bool success, int identifier, string deviceToken, string payload);
	

	public class AppleTestServer
	{
		NotificationReceivedDelegate NotificationReceived;

		TcpListener listener;
		List<ApnsClient> clients;
		public int ExpectedNotificationLength { get; set; }


		public List<ApnsClient> Clients
		{
			get { return clients; }
		}

		public AppleTestServer(int expectedNotificationLength, NotificationReceivedDelegate notificationReceived)
		{
			this.ExpectedNotificationLength = expectedNotificationLength;
			this.NotificationReceived = notificationReceived;
			clients = new List<ApnsClient>();

			
			this.ResponseFilters = new List<ApnsResponseFilter>();
		}

		public long Received
		{
			get { return Interlocked.Read(ref receivedCounter); }
		}

		public List<ApnsResponseFilter> ResponseFilters { get; set; }

		public void Start()
		{
			listener = new TcpListener(IPAddress.Any, 2195);
			listener.Start();

			Accept();
		}

		public void Stop()
		{
			listener.Stop();

		}

		public void Dispose()
		{
			Stop();
		}

		long receivedCounter = 0;

		void Accept()
		{
			var tcpClient = listener.AcceptTcpClient();

			Console.WriteLine("Accepted Connection...");

			var client = new ApnsClient(this, tcpClient, this.NotificationReceived, ac =>
			{
				clients.Remove(ac);
				Console.WriteLine("Client Removed (NewCount: " + clients.Count + ")");
			});

			clients.Add(client);
			Console.WriteLine("Client Added (NewCount: " + clients.Count + ")");

			Task.Factory.StartNew(() => client.Read()).ContinueWith(t =>
			{
				var ex = t.Exception;
				Console.WriteLine("READ EX: " + ex.Message + " Disconnected");
			});

			Accept();
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

	public class ApnsClient
	{
		

		public ApnsClient(AppleTestServer server, TcpClient client, NotificationReceivedDelegate notificationRecd, Action<ApnsClient> conClosed)
		{
			this.notificationReceived = notificationRecd;
			this.connectionClosedAction = conClosed;
			this.tcpClient = client;
			this.server = server;
			this.stream = client.GetStream();
		}

		object lockObj = new object();

		Random rnd = new Random();
		NotificationReceivedDelegate notificationReceived;
		private Action<ApnsClient> connectionClosedAction;

		TcpClient tcpClient;
		private AppleTestServer server;

		bool newMsg = true;

		List<int> identifiers = new List<int>();

		public int GetNumRecd()
		{
			return identifiers.Count;
		}

		public long GetRecdByteCount()
		{
			return recdByteCount;
		}

		public List<int> GetIdGaps()
		{
			var missing = new List<int>();

			var sorted = (from i in identifiers orderby i select i).ToList();

			int last = 0;

			for (int i = 0; i < sorted.Count; i++)
			{
				if (i == 0)
				{
					last = sorted[i];
					continue;
				}

				if (last + 1 != sorted[i])
					missing.Add(last + 1);

				last = sorted[i];

			}

			return missing;
		}

		System.IO.Stream stream;

		List<byte> recdBuffer = new List<byte>();

		private bool isDead = false;

		private long recdByteCount = 0;

		public void Read()
		{
			var buffer = new byte[this.server.ExpectedNotificationLength];
			
			var recd = stream.Read(buffer, 0, buffer.Length);
			recdByteCount += recd;

			while (recd > 0 && !isDead)
			{

				//Add the data to our received buffer
				for (int i = 0; i < recd; i++)
					recdBuffer.Add(buffer[i]);



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

						
						identifiers.Add(identifier);
						

						recdBuffer.RemoveRange(0, msgLength);





						//	System.Threading.Thread.Sleep(rnd.Next(10, 100));

						bool hadError = false;

						foreach (var f in server.ResponseFilters)
						{
							if (f.IsMatch(identifier, token, payload))
							{
								Console.WriteLine("ID: " + identifier + ", Token: " + token + " - BAD TOKEN");
								var b2 = new byte[6];

								b2[0] = 0x01;

								b2[1] = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)f.Status))[0];

								Buffer.BlockCopy(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(identifier)), 0, b2, 2, 4);

								stream.Write(b2, 0, b2.Length);



								try { stream.Close(); }
								catch { }
								try { stream.Dispose(); }
								catch { }

								try { tcpClient.Client.Shutdown(SocketShutdown.Both); }
								catch { }
								try { tcpClient.Close(); }
								catch { }

								hadError = true;
								break;
							}
						}

						//Process the full msg
						if (hadError)
							isDead = true;
						
						if (notificationReceived != null)
							notificationReceived(!hadError, identifier, token, payload);
					}

				}

				recd = stream.Read(buffer, 0, buffer.Length);
				recdByteCount += recd;

			}
			
		}
	}
}
