using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PushSharp;
using PushSharp.Apple;

namespace PushSharp.Tests
{
	[TestFixture]
    public class CoreTests
	{
		private PushBroker broker;
		private byte[] appleCert;

		[SetUp]
		public void Setup()
		{
			appleCert = File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../../Resources/PushSharp.Apns.Sandbox.p12"));
			broker = new PushBroker(false);
		}
		
		[Test]
		public void TestAppleRegistration()
		{
			broker.RegisterAppleService(new ApplePushChannelSettings(false, appleCert, "pushsharp"));
			Assert.IsNotEmpty(broker.GetRegistrations<AppleNotification>());
		}
    }
}
