using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Net.Security;
using System.Xml;
using System.Text;
using System.IO;
using System.Xml.Linq;
using PushSharp.Core;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;

namespace PushSharp.Google
{
    public class GcmXmppConnection
    {
        public const string STREAM_ELEMENT_NAME = "stream";
        public const string SASL_AUTH_ELEMENT_NAME = "auth";
        public const string BIND_ELEMENT_NAME = "bind";
        public const string IQ_ELEMENT_NAME = "iq";
        public const string MESSAGE_ELEMENT_NAME = "message";
        public const string RESOURCE_ELEMENT_NAME = "resource";
        public const string MECHANISMS_ELEMENT_NAME = "mechanisms";
        public const string STREAM_PREFIX = "stream";
        public const string STREAM_NAMESPACE = "http://etherx.jabber.org/streams";
        public const string STREAM_PARAMS_NAMESPACE = "urn:ietf:params:xml:ns:xmpp-streams";
        public const string JABBER_NAMESPACE = "jabber:client";
        public const string GCM_MSG_NAMESPACE = "google:mobile:data";
        public const string SASL_NAMESPACE = "urn:ietf:params:xml:ns:xmpp-sasl";
        public const string BIND_NAMESPACE = "urn:ietf:params:xml:ns:xmpp-bind";
        public const int MAJOR_VERSION = 1;
        public const int MINOR_VERSION = 0;

        readonly XNamespace GCM_NS = GCM_MSG_NAMESPACE;

        X509CertificateCollection certificates;

        TaskCompletionSource<bool> authCompletion;

        public Dictionary<string, CompletableNotification> notifications;

        public GcmXmppConnection (GcmXmppConfiguration configuration)
        {
            authCompletion = new TaskCompletionSource<bool> ();

            notifications = new Dictionary<string,CompletableNotification> ();
            Configuration = configuration;

            certificates = new X509CertificateCollection ();

            // Add local/machine certificate stores to our collection if requested
            //if (Configuration.AddLocalAndMachineCertificateStores) {
                var store = new X509Store (StoreLocation.LocalMachine);
                certificates.AddRange (store.Certificates);

                store = new X509Store (StoreLocation.CurrentUser);
                certificates.AddRange (store.Certificates);
            //}

            // Add optionally specified additional certs into our collection
//            if (Configuration.AdditionalCertificates != null) {
//                foreach (var addlCert in Configuration.AdditionalCertificates)
//                    certificates.Add (addlCert);
//            }

            // Finally, add the main private cert for authenticating to our collection
//            if (certificate != null)
//                certificates.Add (certificate);
        }

        public delegate void ReceiveMessageDelegate ();
        public event ReceiveMessageDelegate ReceiveMessage;

        public GcmXmppConfiguration Configuration { get; private set; }

        TcpClient client;
        SslStream sslStream;
        Stream stream;

        XmlWriter xml;
        bool exit = false;
        readonly object writeLock = new object ();

        public Task<bool> Connect ()
        {
            lock (writeLock) {
                client = new TcpClient ();

                Log.Debug ("GCM-XMPP: Connecting...");

                //await client.ConnectAsync (Configuration.Host, Configuration.Port).ConfigureAwait (false);
         
                client.Connect (Configuration.Host, Configuration.Port);

                Log.Debug ("GCM-XMPP: Connected.  Creating Secure Channel...");

                sslStream = new SslStream (client.GetStream (), true, (sender, certificate, chain, sslPolicyErrors) => true);

                Log.Debug ("GCM-XMPP: Authenticating Tls...");

                sslStream.AuthenticateAsClient (Configuration.Host, certificates, System.Security.Authentication.SslProtocols.Tls, false);
                stream = sslStream;

                Log.Debug ("GCM-XMPP: Authenticated Tls.");

                var xws = new XmlWriterSettings {
                    Async = true,
                    OmitXmlDeclaration = true,
                    Indent = true,
                    NewLineHandling = NewLineHandling.None,
                    Encoding = new UTF8Encoding (false),
                    CloseOutput = false,               
                };

                xml = XmlWriter.Create (stream, xws);

                Log.Debug ("GCM-XMPP: Writing opening element...");

                //Write initial stream:stream element
                xml.WriteStartElement (STREAM_PREFIX, STREAM_ELEMENT_NAME, STREAM_NAMESPACE);
                xml.WriteAttributeString (string.Empty, "to", string.Empty, "gcm.googleapis.com");
                xml.WriteAttributeString ("version", string.Format ("{0}.{1}", MAJOR_VERSION, MINOR_VERSION));
                xml.WriteAttributeString ("xmlns", JABBER_NAMESPACE);
                xml.WriteWhitespace ("\n");
                xml.Flush ();

                Log.Debug ("GCM-XMPP: Starting Listening...");
            }

            Task.Factory.StartNew (Listen);

            Log.Debug ("GCM-XMPP: Waiting for Authentication...");
            return authCompletion.Task;
        }

        public void Authenticate ()
        {
//            <auth mechanism="PLAIN" xmlns="urn:ietf:params:xml:ns:xmpp-sasl">Auth Token</auth>
            Log.Debug ("GCM-XMPP: Authenticating...");

            XNamespace ns = SASL_NAMESPACE;
            var el = new XElement (ns + SASL_AUTH_ELEMENT_NAME, new XAttribute ("mechanism", "PLAIN"), Configuration.SaslAuthToken);
            WriteElement (el);
        }

        public void Send (CompletableNotification notification)
        {

            XNamespace ns = GCM_MSG_NAMESPACE;
            var gcm = new XElement (ns + "gcm", notification.Notification.ToJson ());
            var msg = new XElement ("message", new XAttribute ("id", string.Empty), gcm);

            Log.Debug ("GCM-XMPP: Sending: " + msg);

            try {
                WriteElement (msg);

                notifications.Add (notification.Notification.MessageId, notification);
            } catch (Exception ex) {
                notification.CompleteFailed (ex);
            }
        }


        void Listen()
        {
            try {
                var xrs = new XmlReaderSettings {
                    //Async = true,
                    CloseInput = false,
                    ConformanceLevel = ConformanceLevel.Fragment,

                    //IgnoreComments = true,
                    //IgnoreWhitespace = true,
    //                XmlResolver = null,
                };

                Log.Debug ("GCM-XMPP: Listening...");

                using (var xmlReader = XmlReader.Create (stream, xrs))
                {
                    while (!exit && xmlReader.Read ()) //await xmlReader.ReadAsync ().ConfigureAwait (false))
                    {
                        Log.Debug ("GCM-XMPP: Read: " + xmlReader.NodeType + " - " + xmlReader.Name);

                        if (xmlReader.NodeType == XmlNodeType.Element)
                        {
                            if (xmlReader.Name == STREAM_PREFIX + ":" + STREAM_ELEMENT_NAME) {

                                Log.Debug ("GCM-XMPP: Stream initialization node received");

                            } else {
                                Log.Debug ("GCM-XMPP: Reading subtree...");

                                var elem = XElement.Load (xmlReader.ReadSubtree ());

                                Log.Debug ("GCM-XMPP: Loaded Subtree: " + elem);

                                switch (elem.Name.LocalName)
                                {
                                case "success": 
                                    authCompletion.TrySetResult (true);
                                    break;
                                case "failed":
                                    authCompletion.TrySetResult (false);
                                    break;
                                case "error":
                                    
                                    //TODO: Check for stanza error GCM
                                    Log.Debug ("XMPPStream error: " + elem.Value);
                                    break;
                                case "features":
                                    Log.Debug ("Got Features");
                                    Authenticate ();
                                    break;
                                case "message":

                                    //TODO: Received a message, let's parse it!

                                    var gcm = elem.Element (GCM_NS + "gcm");

                                    var err = elem.Element ("error");

                                    if (err != null) {
                                        //TODO: Handle stanza error
                                    } else {
                                        HandleMessage (gcm.Value);
                                    }
                                    break;
                                default:
                                    break;
                                }

                                Log.Debug ("GCM-XMPP: Received: " + elem);
                            }
                        
                        }
                        else if (xmlReader.NodeType == XmlNodeType.EndElement && xmlReader.Name == STREAM_PREFIX + ":" + STREAM_ELEMENT_NAME)
                        {
                            Log.Debug ("GCM-XMPP: Server closed the stream");
                            Close();
                            break;
                        } else {
                            Log.Debug ("GCM-XMPP: WHAT?");
                        }




                        
                    }


                } 
            } catch (Exception ex) {

                Log.Error ("GCM-XMPP: Listener Error {0}", ex);
                authCompletion.TrySetResult (false);
            }

            // If there are any notifications we're waiting on, they need to be failed
            foreach (var n in notifications.Values)
                n.CompleteFailed (new Exception ("Connection Closed before response was received"));

            Log.Debug ("GCM-XMPP: Closed Listener");
        }

        public void Close ()
        {
           // exit = true;

            Log.Debug ("GCM-XMPP: Closing XMPP Stream");
            xml.WriteEndDocument ();
            xml.Flush ();
        }

        void WriteElement(XElement element)
        {
            lock (writeLock) {
                Log.Debug ("GCM-XMPP: Sending: " + element);

                element.WriteTo (xml);
                xml.Flush ();
            }
        }


        void HandleMessage (string json) 
        {
            Log.Debug ("Incoming Message: " + json);
        }

        public class CompletableNotification
        {
            public CompletableNotification (GcmXmppNotification notification)
            {
                Notification = notification;
                completionSource = new TaskCompletionSource<Exception> ();
            }

            public GcmXmppNotification Notification { get; private set; }

            readonly TaskCompletionSource<Exception> completionSource;

            public async Task<Exception> WaitForComplete ()
            {
                return await completionSource.Task.ConfigureAwait (false);
            }

            public void CompleteSuccessfully ()
            {
                completionSource.SetResult (null);
            }

            public void CompleteFailed (Exception ex)
            {
                completionSource.SetResult (ex);
            }
        }

    }
}

