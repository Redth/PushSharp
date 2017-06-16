using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

namespace PushSharp.Tests
{
    public class Settings
    {
        static Settings instance = null;

        public static Settings Instance {
            get {
                if (instance == null) {

                    var envData = Environment.GetEnvironmentVariable ("TEST_CONFIG_JSON");

                    if (!string.IsNullOrEmpty (envData)) {
                        instance = JsonConvert.DeserializeObject<Settings> (envData);
                        return instance;
                    }

                    var baseDir = AppDomain.CurrentDomain.BaseDirectory;

                    var settingsFile = Path.Combine (baseDir, "settings.json");

                    if (!File.Exists (settingsFile))
                        settingsFile = Path.Combine (baseDir, "../", "settings.json");
                    if (!File.Exists (settingsFile))
                        settingsFile = Path.Combine (baseDir, "../../", "settings.json");
                    if (!File.Exists (settingsFile))
                        settingsFile = Path.Combine (baseDir, "../../../", "settings.json");

                    if (!File.Exists (settingsFile))
                        throw new FileNotFoundException ("You must provide a settings.json file to run these tests.  See the settings.json.sample file for more information.");
                    
                    instance = JsonConvert.DeserializeObject<Settings> (File.ReadAllText (settingsFile));
                }
                return instance;
            }
        }
        public Settings ()
        {
        }

        [JsonProperty ("apns_cert_file")]
        public string ApnsCertificateFile { get; set; }
        [JsonProperty ("apns_cert_pwd")]
        public string ApnsCertificatePassword { get;set; }
        [JsonProperty ("apns_device_tokens")]
        public List<string> ApnsDeviceTokens { get;set; }

        [JsonProperty ("gcm_auth_token")]
        public string GcmAuthToken { get;set; }
        [JsonProperty ("gcm_sender_id")]
        public string GcmSenderId { get;set; }
        [JsonProperty ("gcm_registration_ids")]
        public List<string> GcmRegistrationIds { get;set; }

        [JsonProperty ("adm_client_id")]
        public string AdmClientId { get;set; }
        [JsonProperty ("adm_client_secret")]
        public string AdmClientSecret { get;set; }
        [JsonProperty ("adm_registration_ids")]
        public List<string> AdmRegistrationIds { get;set; }

        [JsonProperty ("wns_package_name")]
        public string WnsPackageName { get;set; }
        [JsonProperty ("wns_package_sid")]
        public string WnsPackageSid { get;set; }
        [JsonProperty ("wns_client_secret")]
        public string WnsClientSecret { get;set; }
        [JsonProperty ("wns_channel_uris")]
        public List<string> WnsChannelUris { get;set; }
    }
}

