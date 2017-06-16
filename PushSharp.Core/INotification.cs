using System;

namespace PushSharp.Core
{
    public interface INotification
    {
        bool IsDeviceRegistrationIdValid ();
        object Tag { get; set; }
    }
}
