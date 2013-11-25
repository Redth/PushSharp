using System;

namespace PushSharp.Web.Interfaces
{
	public interface IPushNotification
	{
		Guid DevicePK { get; set; }
		string DeviceModel { get; set; }
		string DeviceName { get; set; }
		string DeviceUUID { get; set; }
		string DeviceNotificationToken { get; set; }
		string Message { get; set; }
		int BadgeNumber { get; set; }
		string SoundFile { get; set; }
		Guid UserPK { get; set; }
		string UserName { get; set; }
		DateTime EventTime { get; set; }
	}
}