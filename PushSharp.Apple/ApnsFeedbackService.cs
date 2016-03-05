using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace PushSharp.Apple
{
    public class FeedbackService
    {
        public FeedbackService (ApnsConfiguration configuration)
        {
            Configuration = configuration;
        }

        public ApnsConfiguration Configuration { get; private set; }

        public delegate void FeedbackReceivedDelegate (string deviceToken, DateTime timestamp);
        public event FeedbackReceivedDelegate FeedbackReceived;

        public void Check ()
        {
            var encoding = Encoding.ASCII;

            var certificate = Configuration.Certificate;

            var certificates = new X509CertificateCollection();
            certificates.Add(certificate);

            var client = new TcpClient (Configuration.FeedbackHost, Configuration.FeedbackPort);

            var stream = new SslStream (client.GetStream(), true,
                (sender, cert, chain, sslErrs) => { return true; },
                (sender, targetHost, localCerts, remoteCert, acceptableIssuers) => { return certificate; });

            stream.AuthenticateAsClient(Configuration.FeedbackHost, certificates, System.Security.Authentication.SslProtocols.Tls, false);


            //Set up
            byte[] buffer = new byte[4096];
            int recd = 0;
            var data = new List<byte> ();

            //Get the first feedback
            recd = stream.Read(buffer, 0, buffer.Length);

            //Continue while we have results and are not disposing
            while (recd > 0)
            {
                // Add the received data to a list buffer to work with (easier to manipulate)
                for (int i = 0; i < recd; i++)
                    data.Add (buffer [i]);
                
                //Process each complete notification "packet" available in the buffer
                while (data.Count >= (4 + 2 + 32)) // Minimum size for a valid packet
                {
                    var secondsBuffer = data.GetRange (0, 4).ToArray ();
                    var tokenLengthBuffer = data.GetRange (4, 2).ToArray ();

                    // Get our seconds since epoch
                    // Check endianness and reverse if needed
                    if (BitConverter.IsLittleEndian)
                        Array.Reverse (secondsBuffer);
                    var seconds = BitConverter.ToInt32 (secondsBuffer, 0);

                    //Add seconds since 1970 to that date, in UTC
                    var timestamp = new DateTime (1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds (seconds);

                    //flag to allow feedback times in UTC or local, but default is local
                    if (!Configuration.FeedbackTimeIsUTC)
                        timestamp = timestamp.ToLocalTime();


                    if (BitConverter.IsLittleEndian)
                        Array.Reverse (tokenLengthBuffer);
                    var tokenLength = BitConverter.ToInt16 (tokenLengthBuffer, 0);

                    if (data.Count >= 4 + 2 + tokenLength) {

                        var tokenBuffer = data.GetRange (6, tokenLength).ToArray ();
                        // Strings shouldn't care about endian-ness... this shouldn't be reversed
                        //if (BitConverter.IsLittleEndian)
                        //    Array.Reverse (tokenBuffer);
                        var token = BitConverter.ToString (tokenBuffer).Replace ("-", "").ToLower ().Trim ();

                        // Remove what we parsed from the buffer
                        data.RemoveRange (0, 4 + 2 + tokenLength);

                        // Raise the event to the consumer
                        var evt = FeedbackReceived;
                        if (evt != null)
                            evt (token, timestamp);
                    } else {
                        continue;
                    }

                }

                //Read the next feedback
                recd = stream.Read (buffer, 0, buffer.Length);
            }

            try
            {
                stream.Close ();
                stream.Dispose();
            }
            catch { }

            try 
            {
                client.Client.Shutdown (SocketShutdown.Both);
                client.Client.Dispose ();
            }
            catch { }

            try { client.Close (); } catch { }

        }
    }
}
