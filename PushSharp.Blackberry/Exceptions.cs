using System;
using System.Collections.Generic;
using PushSharp.Core;

namespace PushSharp.Blackberry
{
    public class BlackberryNotificationException : NotificationException
    {
        public BlackberryNotificationException (BlackberryMessageStatus msgStatus, string desc, BlackberryNotification notification) 
            :base (desc + " - " + msgStatus, notification)
        {
            MessageStatus = msgStatus;
        }

        public BlackberryMessageStatus MessageStatus { get; private set; }
    }
}

