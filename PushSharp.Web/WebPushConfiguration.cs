namespace PushSharp.Web
{
    public class WebPushConfiguration
    {
        public WebPushConfiguration()
        {
            GcmEndPoint = "https://android.googleapis.com/gcm/send";
        }

        public string GcmAPIKey { get; set; }

        internal string GcmEndPoint { get; set; }
    }
}