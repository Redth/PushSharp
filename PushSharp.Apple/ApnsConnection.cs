using System;
using System.IO;
using System.Net.Sockets;
using System.Net.Security;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;
using System.Threading;
using System.Net;
using PushSharp.Core;

namespace PushSharp.Apple
{
    public class ApnsConnection
    {
        static int ID = 0;

        public ApnsConnection (ApnsConfiguration configuration)
        {
            id = ++ID;
            if (id >= int.MaxValue)
                ID = 0;

            Configuration = configuration;

            certificate = Configuration.Certificate;

            certificates = new X509CertificateCollection ();

            // Add local/machine certificate stores to our collection if requested
            if (Configuration.AddLocalAndMachineCertificateStores) {
                var store = new X509Store (StoreLocation.LocalMachine);
                certificates.AddRange (store.Certificates);

                store = new X509Store (StoreLocation.CurrentUser);
                certificates.AddRange (store.Certificates);
            }

            // Add optionally specified additional certs into our collection
            if (Configuration.AdditionalCertificates != null) {
                foreach (var addlCert in Configuration.AdditionalCertificates)
                    certificates.Add (addlCert);
            }

            // Finally, add the main private cert for authenticating to our collection
            if (certificate != null)
                certificates.Add (certificate);

            timerBatchWait = new Timer (new TimerCallback (async state => {

                await batchSendSemaphore.WaitAsync ();
                try {
                    await SendBatch ().ConfigureAwait (false);
                } finally {
                    batchSendSemaphore.Release ();
                }

            }), null, Timeout.Infinite, Timeout.Infinite);
        }

        public ApnsConfiguration Configuration { get; private set; }

        X509CertificateCollection certificates;
        X509Certificate2 certificate;
        TcpClient client;
        SslStream stream;
        Stream networkStream;
        byte[] buffer = new byte[6];
        int id;


        SemaphoreSlim connectingSemaphore = new SemaphoreSlim (1);
        SemaphoreSlim batchSendSemaphore = new SemaphoreSlim (1);
        object notificationBatchQueueLock = new object ();

        //readonly object connectingLock = new object ();
        Queue<CompletableApnsNotification> notifications = new Queue<CompletableApnsNotification> ();
        List<SentNotification> sent = new List<SentNotification> ();

        Timer timerBatchWait;

        public void Send (CompletableApnsNotification notification)
        {
            lock (notificationBatchQueueLock) {

                notifications.Enqueue (notification);

                if (notifications.Count >= Configuration.InternalBatchSize) {

                    // Make the timer fire immediately and send a batch off
                    timerBatchWait.Change (0, Timeout.Infinite);
                    return;
                }

                // Restart the timer to wait for more notifications to be batched
                //  This timer will keep getting 'restarted' before firing as long as notifications
                //  are queued before the timer's due time
                //  if the timer is actually called, it means no more notifications were queued, 
                //  so we should flush out the queue instead of waiting for more to be batched as they
                //  might not ever come and we don't want to leave anything stranded in the queue
                timerBatchWait.Change (Configuration.InternalBatchingWaitPeriod, Timeout.InfiniteTimeSpan);
            }
        }

        long batchId = 0;

        async Task SendBatch ()
        {
            batchId++;
            if (batchId >= long.MaxValue)
                batchId = 1;
            
            // Pause the timer
            timerBatchWait.Change (Timeout.Infinite, Timeout.Infinite);

            if (notifications.Count <= 0)
                return;

            // Let's store the batch items to send internally
            var toSend = new List<CompletableApnsNotification> ();

            while (notifications.Count > 0 && toSend.Count < Configuration.InternalBatchSize) {
                var n = notifications.Dequeue ();
                toSend.Add (n);
            }


            Log.Info ("APNS-Client[{0}]: Sending Batch ID={1}, Count={2}", id, batchId, toSend.Count);

            try {

                var data = createBatch (toSend);

                if (data != null && data.Length > 0) {

                    for (var i = 0; i <= Configuration.InternalBatchFailureRetryCount; i++) {

                        await connectingSemaphore.WaitAsync ();

                        try {
                            // See if we need to connect
                            if (!socketCanWrite () || i > 0)
                                await connect ();
                        } finally {
                            connectingSemaphore.Release ();
                        }
                
                        try {
                            await networkStream.WriteAsync(data, 0, data.Length).ConfigureAwait(false);
                            break;
                        } catch (Exception ex) when (i != Configuration.InternalBatchFailureRetryCount) {
                            Log.Info("APNS-CLIENT[{0}]: Retrying Batch: Batch ID={1}, Error={2}", id, batchId, ex);
                        }
                    }

                    foreach (var n in toSend)
                        sent.Add (new SentNotification (n));
                }

            } catch (Exception ex) {
                Log.Error ("APNS-CLIENT[{0}]: Send Batch Error: Batch ID={1}, Error={2}", id, batchId, ex);
                foreach (var n in toSend)
                    n.CompleteFailed (new ApnsNotificationException (ApnsNotificationErrorStatusCode.ConnectionError, n.Notification, ex));
            }

            Log.Info ("APNS-Client[{0}]: Sent Batch, waiting for possible response...", id);

            try {
                await Reader ();
            } catch (Exception ex) {
                Log.Error ("APNS-Client[{0}]: Reader Exception: {1}", id, ex);
            }

            Log.Info ("APNS-Client[{0}]: Done Reading for Batch ID={1}, reseting batch timer...", id, batchId);

            // Restart the timer for the next batch
            timerBatchWait.Change (Configuration.InternalBatchingWaitPeriod, Timeout.InfiniteTimeSpan);
        }

        byte[] createBatch (List<CompletableApnsNotification> toSend)
        {
            if (toSend == null || toSend.Count <= 0)
                return null;
            
            var batchData = new List<byte> ();

            // Add all the frame data
            foreach (var n in toSend)
                batchData.AddRange (n.Notification.ToBytes ());
            
            return batchData.ToArray ();
        }

        async Task Reader ()
        {
            var readCancelToken = new CancellationTokenSource ();

            // We are going to read from the stream, but the stream *may* not ever have any data for us to
            // read (in the case that all the messages sent successfully, apple will send us nothing
            // So, let's make our read timeout after a reasonable amount of time to wait for apple to tell
            // us of any errors that happened.
            readCancelToken.CancelAfter (750);

            int len = -1;

            while (!readCancelToken.IsCancellationRequested) {

                // See if there's data to read
                if (client.Client.Available > 0) {
                    Log.Info ("APNS-Client[{0}]: Data Available...", id);
                    len = await networkStream.ReadAsync (buffer, 0, buffer.Length).ConfigureAwait (false);
                    Log.Info ("APNS-Client[{0}]: Finished Read.", id);
                    break;
                }

                // Let's not tie up too much CPU waiting...
                await Task.Delay (50).ConfigureAwait (false);
            }

            Log.Info ("APNS-Client[{0}]: Received {1} bytes response...", id, len);

            // If we got no data back, and we didn't end up canceling, the connection must have closed
            if (len == 0) {

                Log.Info ("APNS-Client[{0}]: Server Closed Connection...", id);

                // Connection was closed
                disconnect ();
                return;

            } else if (len < 0) { //If we timed out waiting, but got no data to read, everything must be ok!

                Log.Info ("APNS-Client[{0}]: Batch (ID={1}) completed with no error response...", id, batchId);

                //Everything was ok, let's assume all 'sent' succeeded
                foreach (var s in sent)
                    s.Notification.CompleteSuccessfully ();

                sent.Clear ();
                return;
            }

            // If we make it here, we did get data back, so we have errors

            Log.Info ("APNS-Client[{0}]: Batch (ID={1}) completed with error response...", id, batchId);

            // If we made it here, we did receive some data, so let's parse the error
            var status = buffer [1];
            var identifier = IPAddress.NetworkToHostOrder (BitConverter.ToInt32 (buffer, 2));

            // Let's handle the failure
            //Get the index of our failed notification (by identifier)
            var failedIndex = sent.FindIndex (n => n.Identifier == identifier);

            // If we didn't find an index for the failed notification, something is wrong
            // Let's just return
            if (failedIndex < 0)
                return;

            // Get all the notifications before the failed one and mark them as sent!
            if (failedIndex > 0) {
                // Get all the notifications sent before the one that failed
                // We can assume that these all succeeded
                var successful = sent.GetRange (0, failedIndex); //TODO: Should it be failedIndex - 1?

                // Complete all the successfully sent notifications
                foreach (var s in successful)
                    s.Notification.CompleteSuccessfully ();

                // Remove all the successful notifications from the sent list
                // This should mean the failed notification is now at index 0
                sent.RemoveRange (0, failedIndex);
            }

            //Get the failed notification itself
            var failedNotification = sent [0];

            //Fail and remove the failed index from the list
            Log.Info ("APNS-Client[{0}]: Failing Notification {1}", id, failedNotification.Identifier);
            failedNotification.Notification.CompleteFailed (
                new ApnsNotificationException (status, failedNotification.Notification.Notification));

            // Now remove the failed notification from the sent list
            sent.RemoveAt (0);

            // The remaining items in the list were sent after the failed notification
            // we can assume these were ignored by apple so we need to send them again
            // Requeue the remaining notifications
            foreach (var s in sent)
                notifications.Enqueue (s.Notification);

            // Clear our sent list
            sent.Clear ();

            // Apple will close our connection after this anyway
            disconnect ();
        }

        bool socketCanWrite ()
        {
            if (client == null)
                return false;

            if (networkStream == null || !networkStream.CanWrite)
                return false;

            if (!client.Client.Connected)
                return false;

            var p = client.Client.Poll (1000, SelectMode.SelectWrite); 

            Log.Info ("APNS-Client[{0}]: Can Write? {1}", id, p);

            return p;
        }

        async Task connect ()
        {            
            if (client != null)
                disconnect ();
            
            Log.Info ("APNS-Client[{0}]: Connecting (Batch ID={1})", id, batchId);

            client = new TcpClient ();

            try {
                await client.ConnectAsync (Configuration.Host, Configuration.Port).ConfigureAwait (false);

                //Set keep alive on the socket may help maintain our APNS connection
                try {
                    client.Client.SetSocketOption (SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                } catch {
                }

                //Really not sure if this will work on MONO....
                // This may help windows azure users
                try {
                    SetSocketKeepAliveValues (client.Client, (int)Configuration.KeepAlivePeriod.TotalMilliseconds, (int)Configuration.KeepAliveRetryPeriod.TotalMilliseconds);
                } catch {
                }
            } catch (Exception ex) {
                throw new ApnsConnectionException ("Failed to Connect, check your firewall settings!", ex);
            }

            // We can configure skipping ssl all together, ie: if we want to hit a test server
            if (Configuration.SkipSsl) {
                networkStream = client.GetStream ();
            } else {

                // Create our ssl stream
                stream = new SslStream (client.GetStream (), 
                    false,
                    ValidateRemoteCertificate,
                    (sender, targetHost, localCerts, remoteCert, acceptableIssuers) => certificate);

                try {
                    stream.AuthenticateAsClient (Configuration.Host, certificates, System.Security.Authentication.SslProtocols.Tls, false);
                } catch (System.Security.Authentication.AuthenticationException ex) {
                    throw new ApnsConnectionException ("SSL Stream Failed to Authenticate as Client", ex);
                }

                if (!stream.IsMutuallyAuthenticated)
                    throw new ApnsConnectionException ("SSL Stream Failed to Authenticate", null);

                if (!stream.CanWrite)
                    throw new ApnsConnectionException ("SSL Stream is not Writable", null);

                networkStream = stream;
            }

            Log.Info ("APNS-Client[{0}]: Connected (Batch ID={1})", id, batchId);
        }

        void disconnect ()
        {            
            Log.Info ("APNS-Client[{0}]: Disconnecting (Batch ID={1})", id, batchId);

            //We now expect apple to close the connection on us anyway, so let's try and close things
            // up here as well to get a head start
            //Hopefully this way we have less messages written to the stream that we have to requeue
            try { stream.Close (); } catch { }
            try { stream.Dispose (); } catch { }

            try { networkStream.Close (); } catch { }
            try { networkStream.Dispose (); } catch { }

            try { client.Client.Shutdown (SocketShutdown.Both); } catch { }
            try { client.Client.Dispose (); } catch { }

            try { client.Close (); } catch { }

            client = null;
            networkStream = null;
            stream = null;

            Log.Info ("APNS-Client[{0}]: Disconnected (Batch ID={1})", id, batchId);
        }

        bool ValidateRemoteCertificate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors policyErrors)
        {
            if (Configuration.ValidateServerCertificate)
                return policyErrors == SslPolicyErrors.None;

            return true;
        }



        public class SentNotification
        {
            public SentNotification (CompletableApnsNotification notification)
            {
                this.Notification = notification;
                this.SentAt = DateTime.UtcNow;
                this.Identifier = notification.Notification.Identifier;
            }

            public CompletableApnsNotification Notification { get; set; }

            public DateTime SentAt { get; set; }

            public int Identifier { get; set; }
        }

        public class CompletableApnsNotification
        {
            public CompletableApnsNotification (ApnsNotification notification)
            {
                Notification = notification;
                completionSource = new TaskCompletionSource<Exception> ();
            }

            public ApnsNotification Notification { get; private set; }

            TaskCompletionSource<Exception> completionSource;

            public Task<Exception> WaitForComplete ()
            {
                return completionSource.Task;
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

        /// <summary>
        /// Using IOControl code to configue socket KeepAliveValues for line disconnection detection(because default is toooo slow) 
        /// </summary>
        /// <param name="tcpc">TcpClient</param>
        /// <param name="KeepAliveTime">The keep alive time. (ms)</param>
        /// <param name="KeepAliveInterval">The keep alive interval. (ms)</param>
        public static void SetSocketKeepAliveValues (Socket socket, int KeepAliveTime, int KeepAliveInterval)
        {
            //KeepAliveTime: default value is 2hr
            //KeepAliveInterval: default value is 1s and Detect 5 times

            uint dummy = 0; //lenth = 4
            byte[] inOptionValues = new byte[System.Runtime.InteropServices.Marshal.SizeOf (dummy) * 3]; //size = lenth * 3 = 12

            BitConverter.GetBytes ((uint)1).CopyTo (inOptionValues, 0);
            BitConverter.GetBytes ((uint)KeepAliveTime).CopyTo (inOptionValues, System.Runtime.InteropServices.Marshal.SizeOf (dummy));
            BitConverter.GetBytes ((uint)KeepAliveInterval).CopyTo (inOptionValues, System.Runtime.InteropServices.Marshal.SizeOf (dummy) * 2);
            // of course there are other ways to marshal up this byte array, this is just one way
            // call WSAIoctl via IOControl

            // .net 3.5 type
            socket.IOControl (IOControlCode.KeepAliveValues, inOptionValues, null);
        }
    }
}
