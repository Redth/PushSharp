using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace PushSharp.Apple
{
    public class ApnsConfiguration
    {
        #region Constants
        const string APNS_SANDBOX_HOST = "gateway.sandbox.push.apple.com";
        const string APNS_PRODUCTION_HOST = "gateway.push.apple.com";

        const string APNS_SANDBOX_FEEDBACK_HOST = "feedback.sandbox.push.apple.com";
        const string APNS_PRODUCTION_FEEDBACK_HOST = "feedback.push.apple.com";

        const int APNS_SANDBOX_PORT = 2195;
        const int APNS_PRODUCTION_PORT = 2195;

        const int APNS_SANDBOX_FEEDBACK_PORT = 2196;
        const int APNS_PRODUCTION_FEEDBACK_PORT = 2196;

        #endregion

        public ApnsConfiguration (ApnsServerEnvironment serverEnvironment, string certificateFile, string certificateFilePwd)
            : this (serverEnvironment, System.IO.File.ReadAllBytes (certificateFile), certificateFilePwd)
        {
        }

        public ApnsConfiguration (ApnsServerEnvironment serverEnvironment, byte[] certificateData, string certificateFilePwd)
            : this (serverEnvironment, new X509Certificate2 (certificateData, certificateFilePwd,
                X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable))
        {
        }

        public ApnsConfiguration (string overrideHost, int overridePort, bool skipSsl = true)
        {
            SkipSsl = skipSsl;

            Initialize (ApnsServerEnvironment.Sandbox, null);

            OverrideServer (overrideHost, overridePort);
        }

        public ApnsConfiguration (ApnsServerEnvironment serverEnvironment, X509Certificate2 certificate)
        {
            Initialize (serverEnvironment, certificate);
        }

        void Initialize (ApnsServerEnvironment serverEnvironment, X509Certificate2 certificate)
        {
            var production = serverEnvironment == ApnsServerEnvironment.Production;

            Host = production ? APNS_PRODUCTION_HOST : APNS_SANDBOX_HOST;
            FeedbackHost = production ? APNS_PRODUCTION_FEEDBACK_HOST : APNS_SANDBOX_FEEDBACK_HOST;
            Port = production ? APNS_PRODUCTION_PORT : APNS_SANDBOX_PORT;
            FeedbackPort = production ? APNS_PRODUCTION_FEEDBACK_PORT : APNS_SANDBOX_FEEDBACK_PORT;

            Certificate = certificate;

            MillisecondsToWaitBeforeMessageDeclaredSuccess = 3000;
            ConnectionTimeout = 10000;
            MaxConnectionAttempts = 3;

            FeedbackIntervalMinutes = 10;
            FeedbackTimeIsUTC = false;

            AdditionalCertificates = new List<X509Certificate2> ();
            AddLocalAndMachineCertificateStores = false;

            CheckIsApnsCertificate ();

            ValidateServerCertificate = false;

            KeepAlivePeriod = TimeSpan.FromMinutes (20);
            KeepAliveRetryPeriod = TimeSpan.FromSeconds (30);

            InternalBatchSize = 1000;
            InternalBatchingWaitPeriod = TimeSpan.FromMilliseconds (750);

            InternalBatchFailureRetryCount = 1;
        }


        void CheckIsApnsCertificate ()
        {
            if (Certificate != null) {
                var issuerName = Certificate.IssuerName.Name;
                var commonName = Certificate.SubjectName.Name;

                if (!issuerName.Contains ("Apple"))
                    throw new ApnsConnectionException ("Your Certificate does not appear to be issued by Apple!  Please check to ensure you have the correct certificate!");
                if (!commonName.Contains ("Apple Push Services:"))
                    throw new ApnsConnectionException ("Your Certificate is not in the new combined Sandbox/Production APNS certificate format, please create a new single certificate to use");

            } else {
                throw new ApnsConnectionException ("You must provide a Certificate to connect to APNS with!");
            }
        }

        public void OverrideServer (string host, int port)
        {
            Host = host;
            Port = port;
        }

        public void OverrideFeedbackServer (string host, int port)
        {
            FeedbackHost = host;
            FeedbackPort = port;
        }

        public string Host { get; private set; }

        public int Port { get; private set; }

        public string FeedbackHost { get; private set; }

        public int FeedbackPort { get; private set; }

        public X509Certificate2 Certificate { get; private set; }

        public List<X509Certificate2> AdditionalCertificates { get; private set; }

        public bool AddLocalAndMachineCertificateStores { get; set; }

        public bool SkipSsl { get; set; }

        public int MillisecondsToWaitBeforeMessageDeclaredSuccess { get; set; }

        public int FeedbackIntervalMinutes { get; set; }

        public bool FeedbackTimeIsUTC { get; set; }

        public bool ValidateServerCertificate { get; set; }

        public int ConnectionTimeout { get; set; }

        public int MaxConnectionAttempts { get; set; }

        /// <summary>
        /// The internal connection to APNS servers batches notifications to send before waiting for errors for a short time.
        /// This value will set a maximum size per batch.  The default value is 1000.  You probably do not want this higher than 7500.
        /// </summary>
        /// <value>The size of the internal batch.</value>
        public int InternalBatchSize { get; set; }

        /// <summary>
        /// How long the internal connection to APNS servers should idle while collecting notifications in a batch to send.
        /// Setting this value too low might result in many smaller batches being used.
        /// </summary>
        /// <value>The internal batching wait period.</value>
        public TimeSpan InternalBatchingWaitPeriod { get; set; }

        /// <summary>
        /// How many times the internal batch will retry to send in case of network failure. The default value is 1.
        /// </summary>
        /// <value>The internal batch failure retry count.</value>
        public int InternalBatchFailureRetryCount { get; set; }

        /// <summary>
        /// Gets or sets the keep alive period to set on the APNS socket
        /// </summary>
        public TimeSpan KeepAlivePeriod { get; set; }

        /// <summary>
        /// Gets or sets the keep alive retry period to set on the APNS socket
        /// </summary>
        public TimeSpan KeepAliveRetryPeriod { get; set; }

        public enum ApnsServerEnvironment {
            Sandbox,
            Production
        }
    }
}