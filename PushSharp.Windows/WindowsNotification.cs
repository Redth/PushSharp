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
	}

	public class WindowsTileNotification : WindowsNotification
	{
		public WindowsTileNotification() : base()
		{
			Visual = new TileVisual();
		}

		public override WindowsNotificationType Type
		{
			get { return WindowsNotificationType.Tile; }
		}

		public WindowsNotificationCachePolicyType? CachePolicy { get; set; }
		public string NotificationTag { get; set; }
	
		public TileVisual Visual { get; set; }
	
		public override string PayloadToString()
		{
			var tile = new XElement("tile");

			if (Visual != null)
				tile.Add(Visual.GenerateXmlElement());

			return tile.ToString();
		}
	}

	public class TileBinding
	{
		public TileBinding()
		{
			Images = new List<TileImage>();
			Texts = new List<TileText>();
		}

		public TileNotificationTemplate TileTemplate { get; set; }
		public string Fallback { get; set; }
		public string Language { get; set; }
		public string BaseUri { get; set; }
		public BrandingType? Branding { get; set; }
		public bool? AddImageQuery { get; set; }

		public List<TileImage> Images { get; set; }
		public List<TileText> Texts { get; set; }

		public XElement GenerateXmlElement()
		{
			var binding = new XElement("binding", new XAttribute("template", this.TileTemplate.ToString()));	

			if (!string.IsNullOrEmpty(Language))
				binding.Add(new XAttribute("lang", XmlEncode(Language)));

			if (Branding.HasValue)
				binding.Add(new XAttribute("branding", XmlEncode(Branding.Value.ToString().ToLowerInvariant())));

			if (!string.IsNullOrEmpty(BaseUri))
				binding.Add(new XAttribute("baseUri", XmlEncode(BaseUri)));

			if (AddImageQuery.HasValue)
				binding.Add(new XAttribute("addImageQuery", AddImageQuery.Value.ToString().ToLowerInvariant()));

			int idOn = 1;

			if (Images != null)
			{
				foreach (var img in Images)
					binding.Add(img.GenerateXmlElement(idOn++));
			}

			idOn = 1;

			if (Texts != null)
			{
				foreach (var text in Texts)
					binding.Add(text.GenerateXmlElement(idOn++));
			}

			return binding;
		}

		protected string XmlEncode(string text)
		{
			return System.Security.SecurityElement.Escape(text);
		}
	}

	public class TileVisual
	{
		public TileVisual()
		{
			Bindings = new List<TileBinding>();
		}

		public int? Version { get; set; }
		public string Language { get; set;}
		public string BaseUri { get; set; }
		public BrandingType? Branding { get; set;}
		public bool? AddImageQuery { get; set; }

		public List<TileBinding> Bindings { get; set; }

		public XElement GenerateXmlElement()
		{
			var visual = new XElement("visual");

			if (Version.HasValue)
				visual.Add(new XAttribute("version", Version.Value.ToString()));

			if (!string.IsNullOrEmpty(Language))
				visual.Add(new XAttribute("lang", XmlEncode(Language)));

			if (!string.IsNullOrEmpty(BaseUri))
				visual.Add(new XAttribute("baseUri", XmlEncode(BaseUri)));

			if (Branding.HasValue)
				visual.Add(new XAttribute("branding", Branding.Value.ToString().ToLowerInvariant()));

			if (AddImageQuery.HasValue)
				visual.Add(new XAttribute("addImageQuery", AddImageQuery.Value.ToString().ToLowerInvariant()));

			if (Bindings != null)
			{
				foreach (var binding in Bindings)
					visual.Add(binding.GenerateXmlElement());
			}

			return visual;
		}

		protected string XmlEncode(string text)
		{
			return System.Security.SecurityElement.Escape(text);
		}
	} 

	public class ToastBinding
	{
		public ToastBinding()
		{
			Texts = new List<ToastText>();
			Images = new List<ToastImage>();
		}

		public ToastNotificationTemplate ToastTemplate { get; set; }
		public List<ToastText> Texts { get; set; }
		public List<ToastImage> Images { get; set; } 
		public string Fallback { get; set; }
		public string Language { get; set; }
		public string BaseUri { get; set; }
		public BrandingType? Branding { get; set; }
		public bool? AddImageQuery { get; set; }

		public XElement GenerateXmlElement()
		{
			var binding = new XElement("binding", new XAttribute("template", ToastTemplate.ToString()));

			if (!string.IsNullOrEmpty(Fallback))
				binding.Add(new XAttribute("lang", XmlEncode(Fallback)));

			if (!string.IsNullOrEmpty(Language))
				binding.Add(new XAttribute("lang", XmlEncode(Language)));

			if (!string.IsNullOrEmpty(BaseUri))
				binding.Add(new XAttribute("baseUri", XmlEncode(BaseUri)));

			if (AddImageQuery.HasValue)
				binding.Add(new XAttribute("addImageQuery", AddImageQuery.Value.ToString().ToLowerInvariant()));

			int idOn = 1;
			if (Images != null)
			{
				foreach (var img in Images)
					binding.Add(img.GenerateXmlElement(idOn));
			}

			idOn = 1;
			if (Texts != null)
			{
				foreach (var text in Texts)
					binding.Add(text.GenerateXmlElement(idOn++));
			}

			return binding;
		}

		protected string XmlEncode(string text)
		{
			return System.Security.SecurityElement.Escape(text);
		}
	}

	public class ToastAudio
	{
		public ToastAudioSource Source { get; set; }
		public bool Loop { get; set; }
		
		public XElement GenerateXmlElement()
		{
			var audio = new XElement("audio");

			if (Source == ToastAudioSource.Silent)
			{
				audio.Add(new XAttribute("silent", "true"));
			}
			else
			{
				if (Loop)
					audio.Add(new XAttribute("loop", "true"));

				//Default sound is LoopingCall, so don't need to add it if that's the case
				if (Source != ToastAudioSource.LoopingCall)
				{
					string audioSrc = null;
					switch (Source)
					{
						case ToastAudioSource.IM:
							audioSrc = "ms-winsoundevent:Notification.IM";
							break;
						case ToastAudioSource.Mail:
							audioSrc = "ms-winsoundevent:Notification.Mail";
							break;
						case ToastAudioSource.Reminder:
							audioSrc = "ms-winsoundevent:Notification.Reminder";
							break;
						case ToastAudioSource.SMS:
							audioSrc = "ms-winsoundevent:Notification.SMS";
							break;
						case ToastAudioSource.LoopingAlarm:
							audioSrc = "ms-winsoundevent:Notification.Looping.Alarm";
							break;
						case ToastAudioSource.LoopingAlarm2:
							audioSrc = "ms-winsoundevent:Notification.Looping.Alarm2";
							break;
						case ToastAudioSource.LoopingCall:
							audioSrc = "ms-winsoundevent:Notification.Looping.Call";
							break;
						case ToastAudioSource.LoopingCall2:
							audioSrc = "ms-winsoundevent:Notification.Looping.Call2";
							break;
					}

					audio.Add(new XAttribute("src", audioSrc));
				}
			}

			return audio;
		}
	}

	public class ToastVisual
	{
		public int? Version { get; set; }
		public string Language { get; set; }
		public string BaseUri { get; set; }
		public BrandingType? Branding { get; set; }
		public bool? AddImageQuery { get; set; }

		public ToastBinding Binding { get; set; }

		public XElement GenerateXmlElement()
		{
			var visual = new XElement("visual");

			if (Version.HasValue)
				visual.Add(new XAttribute("version", Version.Value.ToString()));

			if (!string.IsNullOrEmpty(Language))
				visual.Add(new XAttribute("lang", XmlEncode(Language)));

			if (!string.IsNullOrEmpty(BaseUri))
				visual.Add(new XAttribute("baseUri", XmlEncode(BaseUri)));

			if (Branding.HasValue)
				visual.Add(new XAttribute("branding", Branding.Value.ToString().ToLowerInvariant()));

			if (AddImageQuery.HasValue)
				visual.Add(new XAttribute("addImageQuery", AddImageQuery.Value.ToString().ToLowerInvariant()));

			if (Binding != null)
				visual.Add(Binding.GenerateXmlElement());

			return visual;
		}

		protected string XmlEncode(string text)
		{
			return System.Security.SecurityElement.Escape(text);
		}
	}

	public class WindowsToastNotification : WindowsNotification
	{
		public WindowsToastNotification()
			: base()
		{	
			Visual = new ToastVisual();
		}

		public override WindowsNotificationType Type
		{
			get { return WindowsNotificationType.Toast; }
		}
		
		public string Launch { get; set; }
		public ToastDuration Duration { get; set; }

		public ToastAudio Audio { get; set; }
		public ToastVisual Visual { get; set; }

		public override string PayloadToString()
		{
			var toast = new XElement("toast");

			if (!string.IsNullOrEmpty(this.Launch))
				toast.Add(new XAttribute("launch", XmlEncode(this.Launch)));

			if (Duration != ToastDuration.Short)
				toast.Add(new XAttribute("duration", Duration.ToString().ToLowerInvariant()));
			
			if (Audio != null)
				toast.Add(Audio.GenerateXmlElement());

			if (Visual != null)
				toast.Add(Visual.GenerateXmlElement());

			return toast.ToString();
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

	public class TileImage
	{
		public string Source { get; set; }
		public string Alt { get; set; }
		public bool? AddImageQuery { get; set; }

		public XElement GenerateXmlElement(int id)
		{
			var img = new XElement("image", new XAttribute("id", id.ToString()),
				new XAttribute("src", Source));
		
			if (!string.IsNullOrEmpty(Alt))
				img.Add(new XAttribute("alt", XmlEncode(Alt)));

			if (AddImageQuery.HasValue)
				img.Add(new XAttribute("addImageQuery", AddImageQuery.Value.ToString().ToLowerInvariant()));

			return img;
		}

		protected string XmlEncode(string text)
		{
			return System.Security.SecurityElement.Escape(text);
		}
	}

	public class ToastImage
	{
		public string Source { get; set; }
		public string Alt { get; set; }
		public bool? AddImageQuery { get; set; }

		public XElement GenerateXmlElement(int id)
		{
			var img = new XElement("image", new XAttribute("id", id.ToString()),
				new XAttribute("src", Source));

			if (!string.IsNullOrEmpty(Alt))
				img.Add(new XAttribute("alt", XmlEncode(Alt)));

			if (AddImageQuery.HasValue)
				img.Add(new XAttribute("addImageQuery", AddImageQuery.Value.ToString().ToLowerInvariant()));

			return img;
		}

		protected string XmlEncode(string text)
		{
			return System.Security.SecurityElement.Escape(text);
		}
	}

	public class TileText
	{
		public string Text { get; set; }
		public string Language { get; set; }

		public XElement GenerateXmlElement(int id)
		{
			var text = new XElement("text", new XAttribute("id", id.ToString()));

			if (!string.IsNullOrEmpty(Language))
				text.Add(new XAttribute("lang", XmlEncode(Language)));

			return text;
		}

		protected string XmlEncode(string text)
		{
			return System.Security.SecurityElement.Escape(text);
		}
	}

	public class ToastText
	{
		public string Text { get; set; }
		public string Language { get; set; }

		public XElement GenerateXmlElement(int id)
		{
			var text = new XElement("text", new XAttribute("id", id.ToString()));

			if (!string.IsNullOrEmpty(Language))
				text.Add(new XAttribute("lang", XmlEncode(Language)));

			return text;
		}

		protected string XmlEncode(string text)
		{
			return System.Security.SecurityElement.Escape(text);
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

	public enum ToastAudioSource
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

	public enum BrandingType
	{
		None = 0,
		Logo = 1,
		Name = 2
	}
	
}
