using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PushSharp.Core
{
	public interface IPushChannelFactory
	{
		PushChannelBase CreateChannel(PushServiceBase pushService);
	}

}
