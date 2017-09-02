namespace PushSharp.Common
{
    public interface INotification
    {
        bool IsDeviceRegistrationIdValid ();
        object Tag { get; set; }
    }
}
