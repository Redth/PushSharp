using System;
using System.Net.Sockets;

namespace PushSharp.Apple
{
	public static class TcpExtensions
	{
		/// <summary>
		/// Using IOControl code to configue socket KeepAliveValues for line disconnection detection(because default is toooo slow) 
		/// </summary>
		/// <param name="tcpc">TcpClient</param>
		/// <param name="keepAliveTime">The keep alive time. (ms)</param>
		/// <param name="keepAliveInterval">The keep alive interval. (ms)</param>
		public static void SetSocketKeepAliveValues(this TcpClient tcpc, int keepAliveTime, int keepAliveInterval)
		{
			//KeepAliveTime: default value is 2hr
			//KeepAliveInterval: default value is 1s and Detect 5 times

			uint dummy = 0; //lenth = 4
			byte[] inOptionValues = new byte[System.Runtime.InteropServices.Marshal.SizeOf(dummy) * 3]; //size = lenth * 3 = 12
			bool OnOff = true;

			BitConverter.GetBytes((uint)(OnOff ? 1 : 0)).CopyTo(inOptionValues, 0);
			BitConverter.GetBytes((uint)keepAliveTime).CopyTo(inOptionValues, System.Runtime.InteropServices.Marshal.SizeOf(dummy));
			BitConverter.GetBytes((uint)keepAliveInterval).CopyTo(inOptionValues, System.Runtime.InteropServices.Marshal.SizeOf(dummy) * 2);
			// of course there are other ways to marshal up this byte array, this is just one way
			// call WSAIoctl via IOControl

			// .net 3.5 type
			tcpc.Client.IOControl(IOControlCode.KeepAliveValues, inOptionValues, null);
		}
	}
}