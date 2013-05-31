using System;
using System.Collections.Generic;

namespace PushSharp.Google.Chrome
{
    public class ChromeNotificationSendFailureException : Exception
    {
        public ChromeNotificationSendFailureException(int code, string message) : base(message)
        {
            Code = code;
        }
        
        public int Code { get; set; }
    }
}
