using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Security;
using System.Net.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PushSharp.Core;
using agsXMPP;
using System.Threading.Tasks;
using agsXMPP.protocol.client;
using agsXMPP.protocol.sasl;
using agsXMPP.Xml.Dom;
using agsXMPP.Factory;
using agsXMPP.Net;
using System.Xml;
using System.Collections.Concurrent;


namespace PushSharp.Android
{
    public class GcmCCSPushChannel : IPushChannel
    {
        private static String GCM_ELEMENT_NAME = "gcm";
        private static String GCM_NAMESPACE = "google:mobile:data";
        private static String SERVER_NAME = "gcm.googleapis.com";
#if DEBUG
        private static String SERVER_URL = "gcm-preprod.googleapis.com";
        private static int PORT = 5236;
#else
                private static String SERVER_URL = "gcm-xmpp.googleapis.com";
        private static int PORT = 5235;
#endif
        private XmppClientConnection _xmpp;
        private GcmCCSPushChannelSettings _gcmSettings;
        private ConcurrentQueue<INotification> _notificationQueue;
        private Dictionary<string, SendNotificationCallbackDelegate> _callbacks;
        private Dictionary<string, INotification> _pendingNotifications;
        private bool _isConnected;
        private bool _isConnecting;
        private long _waitCounter = 0;
        //Indicates whether the connection is in draining state, which means that it
        // will not accept any new downstream messages.
        private volatile bool _connectionDraining = false;
        private System.Timers.Timer _keepConnectedTimer;

        public event GcmMessageReceivedDelegate OnGcmMessageReceived;



        private string Username
        {
            get
            {
                return _gcmSettings.SenderID + "@" + SERVER_NAME;
            }
        }
        private string Password
        {
            get
            {
                return _gcmSettings.SenderAuthToken;
            }
        }


        public GcmCCSPushChannel(GcmCCSPushChannelSettings channelSettings)
        {
            _gcmSettings = channelSettings;
            _callbacks = new Dictionary<string, SendNotificationCallbackDelegate>();
            _pendingNotifications = new Dictionary<string, INotification>();
            _notificationQueue = new ConcurrentQueue<INotification>();

            var mechanism = SaslFactory.GetMechanism("MyPLAINMechanism");
            if (mechanism == null)
            {

                SaslFactory.AddMechanism("MyPLAINMechanism", typeof(MyPlainMechanismClass));
            }

            Connect();
            _keepConnectedTimer = new System.Timers.Timer(10000);
            _keepConnectedTimer.Elapsed += _keepConnectedTimer_Elapsed;
            _keepConnectedTimer.AutoReset = false;
            _keepConnectedTimer.Start();
        }

        void _keepConnectedTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            var timer = (System.Timers.Timer)sender;
            timer.Stop();
            try
            {
                if (_isConnected || _isConnecting)
                    return;
                Log.Info("{0}", "Reconnecting to Google GCM xmpp server");
                Connect();
            }
            finally
            {
                timer.Start();
            }
        }

        private void Connect()
        {
            try
            {
                _isConnecting = true;

                _xmpp = new XmppClientConnection
                {
                    UseSSL = true,
                    UseStartTLS = false,
                    Server = SERVER_URL,
                    ConnectServer = SERVER_URL,
                    Port = PORT,
                    Username = Username,
                    Password = Password,
                    AutoResolveConnectServer = false,
                    SocketConnectionType = SocketConnectionType.Direct,
                    KeepAlive = true,
                };
                _xmpp.OnClose += xmpp_OnClose;
                _xmpp.OnMessage += xmpp_OnMessage;
                _xmpp.OnAuthError += OnAuthError;
                _xmpp.OnError += OnError;
                _xmpp.OnSocketError += xmpp_OnSocketError;
                _xmpp.OnStreamError += xmpp_OnStreamError;
                _xmpp.OnBinded += xmpp_OnBinded;

                _xmpp.Open();
                _xmpp.OnSaslStart += (sender, args) =>
                {
                    args.Auto = false;
                    args.Mechanism = "MyPLAINMechanism";
                    args.ExtentedData = new GcmPlainSaslExtendedData
                    {
                        Username = this.Username,
                        Password = this.Password
                    };
                };
            }
            catch (Exception ex)
            {
                Log.Error("{0} - {1}", "Error in connecting to Google Gcm CCS", ex);
                //  throw;
            }
            finally
            {
                _isConnecting = false;
            }
        }



        static String createJsonAck(String to, String messageId)
        {
            JObject message = new JObject();
            message["message_type"] = "ack";
            message["to"] = to;
            message["message_id"] = messageId;
            return message.ToString();
        }

        void xmpp_OnMessage(object sender, Message msg)
        {
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(msg.InnerXml);

                JObject jsonObject = JObject.Parse(xml.InnerText);

                // present for "ack"/"nack", null otherwise
                Object messageType = jsonObject["message_type"];

                if (messageType == null)
                {
                    // Normal upstream data message
                    HandleUpstreamMessage(jsonObject);

                    SendAck(jsonObject);
                }
                else if ("ack".Equals(messageType.ToString()))
                {
                    // Process Ack
                    HandleAckReceipt(jsonObject);
                }
                else if ("nack".Equals(messageType.ToString()))
                {
                    // Process Nack
                    handleNackReceipt(jsonObject);
                }
                else if ("control".Equals(messageType.ToString()))
                {
                    // Process control message
                    handleControlMessage(jsonObject);
                }
                else if ("receipt".Equals(messageType.ToString()))
                {
                    handleMessageReceipt(jsonObject);
                }
                else
                {
                    Log.Warning("{0}->{1}", "Unrecognized message type", messageType.ToString());
                }

            }
            catch (Exception e)
            {
                Log.Error("{0}->{1}", "Failed to process packet", e);
            }

        }

        private void SendAck(JObject jsonObject)
        {
            // Send ACK to CCS
            String messageId = (String)jsonObject["message_id"];
            String from = (String)jsonObject["from"];
            String ack = createJsonAck(from, messageId);
            // send(ack);
            _xmpp.Send(CreateXML(ack, messageId));
        }

        private void handleMessageReceipt(JObject jsonObject)
        {
            SendAck(jsonObject);
            //TODO: Handle message receipt

            //JToken data = jsonObject.GetValue("data");
            //String messageId = (string)data["original_message_id"];
            //GcmCCSNotification notification = (GcmCCSNotification)_pendingNotifications[messageId];
            //if (notification != null)
            //{
            //    SendNotificationCallbackDelegate callback = _callbacks[messageId];
            //    callback(this, new SendNotificationResult(notification));

            //    if (notification.RequestDeliveryReceipt.GetValueOrDefault(false) == true)
            //    {
            //        _callbacks.Remove(messageId);
            //        _pendingNotifications.Remove(messageId);
            //    }
            //}
        }

        /**
        * Handles an ACK.
        *
        * <p>Logs a INFO message, but subclasses could override it to
        * properly handle ACKs.
        */
        protected void HandleAckReceipt(JObject jsonObject)
        {
            String messageId = (String)jsonObject["message_id"];
            String from = (String)jsonObject["from"];
            String CanonicalRegistrationId = (String)jsonObject["registration_id"];

            Log.Info("handleAckReceipt from {0},messageID: {1}", from, messageId);
            SendNotificationCallbackDelegate callback = _callbacks[messageId];

            GcmCCSNotification notification = (GcmCCSNotification)_pendingNotifications[messageId];
            SendNotificationResult result = null;
            if (!String.IsNullOrEmpty(CanonicalRegistrationId))
            {
                result = new SendNotificationResult(notification, false, new DeviceSubscriptonExpiredException())
                {
                    OldSubscriptionId = notification.To,
                    NewSubscriptionId = CanonicalRegistrationId,
                    IsSubscriptionExpired = true
                };
            }
            else
                result = new SendNotificationResult(notification);

            //if (notification.RequestDeliveryReceipt.GetValueOrDefault(false) == false)
            //{
            callback(this, result);
            _callbacks.Remove(messageId);
            _pendingNotifications.Remove(messageId);
            //}
        }

        /**
         * Handles a NACK.
         *
         * <p>Logs a INFO message, but subclasses could override it to
         * properly handle NACKs.
         */
        protected void handleNackReceipt(JObject jsonObject)
        {
            String messageId = (String)jsonObject["message_id"];
            String from = (String)jsonObject["from"];
            string error = (String)jsonObject["error"];
            string errorDescription = (String)jsonObject["error_description"];
            GcmCCSMessageTransportResponseStatus status = ParseError(error);
            SendNotificationCallbackDelegate callback = _callbacks[messageId];
            GcmCCSNotification notification = (GcmCCSNotification)_pendingNotifications[messageId];
            switch (status)
            {
                case GcmCCSMessageTransportResponseStatus.DEVICE_UNREGISTERED:
                    {

                        callback(this, new SendNotificationResult(notification, false, new DeviceSubscriptonExpiredException()) { OldSubscriptionId = notification.To, IsSubscriptionExpired = true, SubscriptionExpiryUtc = DateTime.UtcNow });
                        break;
                    }
                case GcmCCSMessageTransportResponseStatus.SERVICE_UNAVAILABLE:
                    callback(this, new SendNotificationResult(notification, true, new Exception("Unavailable Response Status")));
                    break;
                default:
                    callback(this, new SendNotificationResult(notification, false, new GcmCCSMessageTransportException(String.Format("{0}: {1}", error, errorDescription), status)));
                    break;
            }
            Log.Info("handleNackReceipt from {0},messageID: {1}", from, messageId);

            //callback(this, new SendNotificationResult(_pendingNotifications[messageId], true, new Exception("Nack received")));
            _callbacks.Remove(messageId);
            _pendingNotifications.Remove(messageId);
        }

        private GcmCCSMessageTransportResponseStatus ParseError(string err)
        {
            if (string.IsNullOrEmpty(err))
                return GcmCCSMessageTransportResponseStatus.OK;

            GcmCCSMessageTransportResponseStatus status = GcmCCSMessageTransportResponseStatus.ERROR;

            Enum.TryParse(err, out status);
            return status;
        }


        protected void handleControlMessage(JObject jsonObject)
        {
            Log.Info("handleControlMessage(): {0}", jsonObject);
            String controlType = (String)jsonObject["control_type"];
            if ("CONNECTION_DRAINING".Equals(controlType))
            {
                _connectionDraining = true;
            }
            else
            {
                Log.Info("Unrecognized control type: {0}. This could happen if new features are added to the CCS protocol.", controlType);
            }
        }

        private void HandleUpstreamMessage(JObject jsonObject)
        {
            if (OnGcmMessageReceived != null)
                OnGcmMessageReceived(this, jsonObject.ToObject<Dictionary<string, object>>());
        }
        private void xmpp_OnClose(object sender)
        {
            _isConnected = false;
        }
        void xmpp_OnBinded(object sender)
        {
            _isConnected = true;
        }

        static void xmpp_OnSocketError(object sender, Exception ex)
        {
            Log.Error("{0}", ex);
            return;
        }
        void xmpp_OnStreamError(object sender, Element e)
        {
            Log.Error("{0}", e);
        }
        static void OnAuthError(object sender, agsXMPP.Xml.Dom.Element e)
        {
            Log.Error("{0}", e);
        }

        static void OnError(object sender, Exception ex)
        {
            Log.Error("{0}", ex);
        }

        public void SendNotification(INotification notification, SendNotificationCallbackDelegate callback)
        {
            try
            {
                var msg = notification as GcmCCSNotification;

                _callbacks[msg.MessageID] = callback;
                _pendingNotifications[msg.MessageID] = notification;
                string xml = CreateXML(msg.GetJson(), msg.MessageID);
                _xmpp.Send(xml);
            }
            catch (Exception ex)
            {
                Log.Error("Error sending android GCM notification", ex);
                callback(this, new SendNotificationResult(notification, true, ex));
            }
        }

        private string CreateXML(string json, string messageID)
        {
            return String.Format("<message id=\"{3}\"><{0} xmlns=\"{1}\">{2}</{0}></message>",
                    GCM_ELEMENT_NAME, GCM_NAMESPACE,
                    System.Security.SecurityElement.Escape(json), messageID);
        }


        public void Dispose()
        {
            var slept = 0;
            while (Interlocked.Read(ref _waitCounter) > 0 && slept <= 5000)
            {
                slept += 100;
                Thread.Sleep(100);
            }
        }

    }
    public class MyPlainMechanismClass : agsXMPP.Sasl.Mechanism
    {
        private XmppClientConnection m_XmppClient = null;

        public void GcmPlainSaslMechanism()
        {

        }

        public override void Init(XmppClientConnection con)
        {
            m_XmppClient = con;

            // <auth mechanism="PLAIN" xmlns="urn:ietf:params:xml:ns:xmpp-sasl">$Message</auth>
            m_XmppClient.Send(new agsXMPP.protocol.sasl.Auth(agsXMPP.protocol.sasl.MechanismType.PLAIN, Message()));
        }

        public override void Parse(Node e)
        {
            // not needed here in PLAIN mechanism
        }

        private string Message()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append((char)0);
            sb.Append(((GcmPlainSaslExtendedData)this.ExtentedData).Username);
            sb.Append((char)0);
            sb.Append(((GcmPlainSaslExtendedData)this.ExtentedData).Password);

            byte[] msg = Encoding.UTF8.GetBytes(sb.ToString());
            return Convert.ToBase64String(msg, 0, msg.Length);
        }
    }
    public class GcmPlainSaslExtendedData : agsXMPP.Sasl.ExtendedData
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

}
