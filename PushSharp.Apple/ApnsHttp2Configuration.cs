using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace PushSharp.Apple
{
    public class ApnsHttp2Configuration
    {
        #region Constants
        const string APNS_SANDBOX_HOST = "api.development.push.apple.com";
        const string APNS_PRODUCTION_HOST = "api.push.apple.com";

        const uint APNS_SANDBOX_PORT = 443;
        const uint APNS_PRODUCTION_PORT = 443;
        #endregion

        public ApnsHttp2Configuration (ApnsServerEnvironment serverEnvironment, string certificateFile, string certificateFilePwd)
            : this (serverEnvironment, System.IO.File.ReadAllBytes (certificateFile), certificateFilePwd)
        {
        }

        public ApnsHttp2Configuration (ApnsServerEnvironment serverEnvironment, byte[] certificateData, string certificateFilePwd)
            : this (serverEnvironment, new X509Certificate2 (certificateData, certificateFilePwd,
                X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable))
        {
        }

        public ApnsHttp2Configuration (string overrideHost, uint overridePort, bool skipSsl = true)
        {
            SkipSsl = skipSsl;

            Initialize (ApnsServerEnvironment.Sandbox, null);

            OverrideServer (overrideHost, overridePort);
        }

        public ApnsHttp2Configuration (ApnsServerEnvironment serverEnvironment, X509Certificate2 certificate)
        {
            Initialize (serverEnvironment, certificate);
        }

        void Initialize (ApnsServerEnvironment serverEnvironment, X509Certificate2 certificate)
        {
            var production = serverEnvironment == ApnsServerEnvironment.Production;

            Host = production ? APNS_PRODUCTION_HOST : APNS_SANDBOX_HOST;
            Port = production ? APNS_PRODUCTION_PORT : APNS_SANDBOX_PORT;

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

        public void OverrideServer (string host, uint port)
        {
            Host = host;
            Port = port;
        }

        public string Host { get; private set; }

        public uint Port { get; private set; }

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