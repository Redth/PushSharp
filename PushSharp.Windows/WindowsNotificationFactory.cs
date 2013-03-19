﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PushSharp.Core;

namespace PushSharp.Windows
{
	public class WindowsNotificationFactory : Notification
	{
		public WindowsToastNotification Toast()
		{
			return new WindowsToastNotification();
		}

		public WindowsTileNotification Tile()
		{
			return new WindowsTileNotification();
		}

		public WindowsRawNotification Raw()
		{
			return new WindowsRawNotification();
		}
		
		public WindowsBadgeNumericNotification BadgeNumeric()
		{
			return new WindowsBadgeNumericNotification();
		}

		public WindowsBadgeGlyphNotification BadgeGlyph()
		{
			return new WindowsBadgeGlyphNotification();
		}
	}
}
