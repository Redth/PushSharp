using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PushSharp.Windows
{
	public abstract class WindowsNotification : Common.Notification
	{
		protected WindowsNotification()
		{
			this.Platform = Common.PlatformType.Windows;
		}

		public string ChannelUri { get; set; }

		public bool? RequestForStatus { get; set; }
		public int? TimeToLive { get; set; }
		public int Version = 1;
		
		public abstract string PayloadToString();

		public abstract WindowsNotificationType Type { get; }

		protected string XmlEncode(string text)
		{
			return System.Security.SecurityElement.Escape(text);
		}

		protected string GeneratePayload(XElement rootElement, WindowsNotification notification, string template, Dictionary<string, string> images, List<string> texts)
		{
			var visual = new XElement("visual");
			var binding = new XElement("binding", new XAttribute("template", template.ToString()));

			if (notification is WindowsTileNotification)
			{
				var tileNotification = notification as WindowsTileNotification;

				visual.Add(new XAttribute("version", tileNotification.Version));
				binding.Add(new XAttribute("version", tileNotification.Version));
				
				if (!string.IsNullOrEmpty(tileNotification.Language))
				{
					visual.Add(new XAttribute("lang", XmlEncode(tileNotification.Language)));
					binding.Add(new XAttribute("lang", XmlEncode(tileNotification.Language)));
				}

				if (!string.IsNullOrEmpty(tileNotification.Branding))
				{
					visual.Add(new XAttribute("branding", XmlEncode(tileNotification.Branding.ToLowerInvariant())));
					binding.Add(new XAttribute("branding", XmlEncode(tileNotification.Branding.ToLowerInvariant())));
				}

				if (!string.IsNullOrEmpty(tileNotification.BaseUri))
				{
					visual.Add(new XAttribute("baseUri", XmlEncode(tileNotification.BaseUri)));
					binding.Add(new XAttribute("baseUri", XmlEncode(tileNotification.BaseUri)));
				}

				if (tileNotification.AddImageQuery)
				{
					visual.Add(new XAttribute("addImageQuery", true));
					binding.Add(new XAttribute("addImageQuery", true));
				}
			}
			else if (notification is WindowsToastNotification)
			{
				var toastNotification = notification as WindowsToastNotification;

				visual.Add(new XAttribute("version", toastNotification.Version));
			
				if (!string.IsNullOrEmpty(toastNotification.Language))
					visual.Add(new XAttribute("lang", XmlEncode(toastNotification.Language)));
			
				if (!string.IsNullOrEmpty(toastNotification.BaseUri))
					visual.Add(new XAttribute("baseUri", XmlEncode(toastNotification.BaseUri)));
			
				if (toastNotification.AddImageQuery)
					visual.Add(new XAttribute("addImageQuery", true));
			}

			

			int idOn = 1;

			foreach (var imgSrc in images.Keys)
			{
				var alt = images[imgSrc];

				var image = new XElement("image", new XAttribute("id", idOn), new XAttribute("src", XmlEncode(imgSrc)));

				if (!string.IsNullOrEmpty(alt))
					image.Add(new XAttribute("alt", XmlEncode(alt)));

				binding.Add(image);

				idOn++;
			}

			idOn = 1;

			foreach (var text in texts)
			{
				binding.Add(new XElement("text", new XAttribute("id", idOn), XmlEncode(text)));
				idOn++;
			}

			visual.Add(binding);
			rootElement.Add(visual);

			//If Toast, add audio
			if (notification is WindowsToastNotification)
			{
				var toastNotification = notification as WindowsToastNotification;

				var audio = new XElement("audio");

				if (toastNotification.AudioType == ToastAudioType.Silent)
				{
					audio.Add(new XAttribute("silent", "true"));
				}
				else
				{
					if (toastNotification.AudioLoop)
						audio.Add(new XAttribute("loop", "true"));

					//Default sound is LoopingCall, so don't need to add it if that's the case
					if (toastNotification.AudioType != ToastAudioType.LoopingCall)
					{
						string audioSrc = null;
						switch (toastNotification.AudioType)
						{
							case ToastAudioType.IM:
								audioSrc = "ms-winsoundevent:Notification.IM";
								break;
							case ToastAudioType.Mail:
								audioSrc = "ms-winsoundevent:Notification.Mail";
								break;
							case ToastAudioType.Reminder:
								audioSrc = "ms-winsoundevent:Notification.Reminder";
								break;
							case ToastAudioType.SMS:
								audioSrc = "ms-winsoundevent:Notification.SMS";
								break;
							case ToastAudioType.LoopingAlarm:
								audioSrc = "ms-winsoundevent:Notification.Looping.Alarm";
								break;
							case ToastAudioType.LoopingAlarm2:
								audioSrc = "ms-winsoundevent:Notification.Looping.Alarm2";
								break;
							case ToastAudioType.LoopingCall:
								audioSrc = "ms-winsoundevent:Notification.Looping.Call";
								break;
							case ToastAudioType.LoopingCall2:
								audioSrc = "ms-winsoundevent:Notification.Looping.Call2";
								break;
						}

						audio.Add(new XAttribute("src", audioSrc));
					}
				}

				rootElement.Add(audio);
			}

			return rootElement.ToString();
		}
	}

	public class WindowsTileNotification : WindowsNotification
	{
		public WindowsTileNotification()
			: base()
		{
			this.Texts = new List<string>();
			this.Images = new Dictionary<string, string>();
			this.TileTemplate = TileNotificationTemplate.TileSquareBlock;
		}

		public override WindowsNotificationType Type
		{
			get { return WindowsNotificationType.Tile; }
		}

		public WindowsNotificationCachePolicyType? CachePolicy { get; set; }
		public string NotificationTag { get; set; }

		public TileNotificationTemplate TileTemplate { get; set; }
		public Dictionary<string, string> Images { get; set; }
		public List<string> Texts { get; set; }
		public string Language { get; set; }
		public TileBrandingType Branding { get; set; }
		public string BaseUri { get; set; }
		public bool AddImageQuery { get; set; }

		public override string PayloadToString()
		{
			return this.GeneratePayload(new XElement("tile"), this, this.TileTemplate.ToString(), Images, Texts);
		}
	}

	public class WindowsToastNotification : WindowsNotification
	{
		public WindowsToastNotification()
			: base()
		{
			this.Texts = new List<string>();
			this.Images = new Dictionary<string, string>();
			this.TextTemplate = ToastNotificationTemplate.ToastImageAndText01;
		}

		public override WindowsNotificationType Type
		{
			get { return WindowsNotificationType.Toast; }
		}

		public ToastNotificationTemplate TextTemplate { get; set; }
		public Dictionary<string, string> Images { get; set; }
		public List<string> Texts { get; set; }

		public string Language { get; set; }
		public string BaseUri { get; set; }
		public bool AddImageQuery { get; set; }

		public string Launch { get; set; }
		public ToastDuration Duration { get; set; }
		public ToastAudioType AudioType { get; set; }
		public bool AudioLoop { get; set; }
		


		public override string PayloadToString()
		{
			var toast = new XElement("toast");

			if (!string.IsNullOrEmpty(this.Launch))
				toast.Add(new XAttribute("launch", XmlEncode(this.Launch)));

			if (Duration != ToastDuration.Short)
				toast.Add(new XAttribute("duration", Duration.ToString().ToLowerInvariant()));
			
			return this.GeneratePayload(toast, this, this.TextTemplate.ToString(), Images, Texts);
		}
	}

	public class WindowsBadgeGlyphNotification : WindowsNotification
	{
		public override WindowsNotificationType Type
		{
			get { return WindowsNotificationType.Badge; }
		}

		public WindowsNotificationCachePolicyType? CachePolicy { get; set; }

		public BadgeGlyphValue Glyph { get; set; }
	
		public override string PayloadToString()
		{
			var badge = new XElement("badge");
			badge.Add(new XAttribute("version", this.Version));
			badge.Add(new XAttribute("value", Glyph.ToString().ToLowerInvariant()));

			return badge.ToString();
		}
	}

	public class WindowsBadgeNumericNotification : WindowsNotification
	{
		public override WindowsNotificationType Type
		{
			get { return WindowsNotificationType.Badge; }
		}

		public WindowsNotificationCachePolicyType? CachePolicy { get; set; }

		public int BadgeNumber { get; set; }

		public override string PayloadToString()
		{
			var badge = new XElement("badge");
			badge.Add(new XAttribute("version", this.Version));
			badge.Add(new XAttribute("value", BadgeNumber.ToString().ToLowerInvariant()));

			return badge.ToString();
		}
	}

	public class WindowsRawNotification : WindowsNotification
	{
		public override WindowsNotificationType Type
		{
			get { return WindowsNotificationType.Raw; }
		}

		public string RawXml { get; set; }

		public override string PayloadToString()
		{
			return RawXml;
		}
	}

	public enum ToastDuration
	{
		Short = 0,
		Long = 1
	}

	public enum ToastAudioType
	{
		/// <summary>
		/// The default toast audio sound.
		/// </summary>
		Default = 0,
		/// <summary>
		/// Audio that corresponds to new mail arriving.
		/// </summary>
		Mail,
		/// <summary>
		/// Audio that corresponds to a new SMS message arriving.
		/// </summary>
		SMS,
		/// <summary>
		/// Audio that corresponds to a new IM arriving.
		/// </summary>
		IM,
		/// <summary>
		/// Audio that corresponds to a reminder.
		/// </summary>
		Reminder,
		/// <summary>
		/// The default looping sound.  Audio that corresponds to a call.
		/// Only valid for toasts that are have the duration set to "Long".
		/// </summary>
		LoopingCall,
		/// <summary>
		/// Audio that corresponds to a call.
		/// Only valid for toasts that are have the duration set to "Long".
		/// </summary>
		LoopingCall2,
		/// <summary>
		/// Audio that corresponds to an alarm.
		/// Only valid for toasts that are have the duration set to "Long".
		/// </summary>
		LoopingAlarm,
		/// <summary>
		/// Audio that corresponds to an alarm.
		/// Only valid for toasts that are have the duration set to "Long".
		/// </summary>
		LoopingAlarm2,
		/// <summary>
		/// No audio should be played when the toast is displayed.
		/// </summary>
		Silent
	}

	public enum BadgeGlyphValue
	{
		/// <summary>
		/// No glyph.  If there is a numeric badge, or a glyph currently on the badge,
		/// it will be removed.
		/// </summary>
		None = 0,
		/// <summary>
		/// A glyph representing application activity.
		/// </summary>
		Activity,
		/// <summary>
		/// A glyph representing an alert.
		/// </summary>
		Alert,
		/// <summary>
		/// A glyph representing availability status.
		/// </summary>
		Available,
		/// <summary>
		/// A glyph representing away status
		/// </summary>
		Away,
		/// <summary>
		/// A glyph representing busy status.
		/// </summary>
		Busy,
		/// <summary>
		/// A glyph representing that a new message is available.
		/// </summary>
		NewMessage,
		/// <summary>
		/// A glyph representing that media is paused.
		/// </summary>
		Paused,
		/// <summary>
		/// A glyph representing that media is playing.
		/// </summary>
		Playing,
		/// <summary>
		/// A glyph representing unavailable status.
		/// </summary>
		Unavailable,
		/// <summary>
		/// A glyph representing an error.
		/// </summary>
		Error,
		/// <summary>
		/// A glyph representing attention status.
		/// </summary>
		Attention
	}

	public enum WindowsNotificationCachePolicyType
	{
		Cache,
		NoCache
	}

	public enum WindowsNotificationType
	{
		Badge,
		Tile,
		Toast,
		Raw
	}

	public enum ToastNotificationTemplate
	{
		ToastText01,
		ToastText02,
		ToastText03,
		ToastText04,
		ToastImageAndText01,
		ToastImageAndText02,
		ToastImageAndText03,
		ToastImageAndText04
	}

	public enum TileBrandingType
	{
		None = 0,
		Logo = 1,
		Name = 2
	}
	
}
