using PushSharp.Common;

namespace PushSharp.WindowsPhone
{
	public class WindowsPhonePushChannelSettings : PushChannelSettings
	{

		public WindowsPhonePushChannelSettings(byte[] webServiceCertificate = null)
		{
			this.WebServiceCertificate = webServiceCertificate;
		}

		public byte[] WebServiceCertificate { get; private set; }

		
	}
}
