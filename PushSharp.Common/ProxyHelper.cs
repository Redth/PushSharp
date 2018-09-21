using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PushSharp.Common
{
    public delegate void BeforeConnectDelegate();

    public delegate void AfterConnectDelegate(string status);

    public class ProxyHelper
    {
        public Func<string, Exception> ProxyConnectionExceptionCreator { get; set; }
        public event BeforeConnectDelegate BeforeConnect;
        public event AfterConnectDelegate AfterConnect;

        public async Task Connect(TcpClient client, string targetHost, int targetPort, string proxyHost, int proxyPort, NetworkCredential proxyCredential = null)
        {
            this.OnBeforeConnect();
            await client.ConnectAsync(proxyHost, proxyPort).ConfigureAwait(false);
            var stream = client.GetStream();
            var authorization = string.Empty;
            if (proxyCredential != null)
            {
                var credential = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{proxyCredential.UserName}:{proxyCredential.Password}"));
                authorization = $"\r\nAuthorization: Basic {credential}";
            }
            var buffer =
                Encoding.UTF8.GetBytes(
                    string.Format("CONNECT {0}:{1}  HTTP/1.1\r\nHost: {0}:{1}{2}\r\nProxy-Connection: keep-alive\r\n\r\n", targetHost, targetPort, authorization));
            await stream.WriteAsync(buffer, 0, buffer.Length);
            await stream.FlushAsync();
            buffer = new byte[client.Client.ReceiveBufferSize];
            using (var resp = new MemoryStream())
            {
                do
                {
                    var len = await stream.ReadAsync(buffer, 0, buffer.Length);
                    resp.Write(buffer, 0, len);
                }
                while (client.Client.Available > 0);
                buffer = resp.ToArray();
            }
            var content = Encoding.UTF8.GetString(buffer);
            var i = content.IndexOf('\r');
            string statusCode;
            if (i > 9)
            {
                statusCode = content.Substring(9, i - 9);
            }
            else
            {
                var max = content.Length;
                if (max > 50)
                {
                    max = 50;
                }
                var append = max == 50 ? "..." : string.Empty;
                statusCode = content.Substring(0, max) + append;
            }
            this.OnAfterConnect(statusCode);
            if (!statusCode.StartsWith("200"))
            {
                var fac = this.ProxyConnectionExceptionCreator;
                if (fac != null)
                {
                    throw fac($"Proxy returned {statusCode}. Check proxy settings and if it allows ssl through ports different of 443.");
                }
                throw new Exception($"Proxy returned {statusCode}. Check proxy settings and if it allows ssl through ports different of 443.");
            }
        }

        protected virtual void OnBeforeConnect()
        {
            this.BeforeConnect?.Invoke();
        }

        protected virtual void OnAfterConnect(string status)
        {
            this.AfterConnect?.Invoke(status);
        }
    }
}