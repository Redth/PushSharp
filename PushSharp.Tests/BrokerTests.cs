using NUnit.Framework;

using System;
using PushSharp.Core;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using PushSharp.Apple;
using Newtonsoft.Json.Linq;
using System.Threading;


namespace PushSharp.Tests
{
	[TestFixture]
    [Category ("Core")]
	public class BrokerTests
	{
		[Test]
        public void Broker_Send_Many ()
		{
            var succeeded = 0;
            var failed = 0;
            var attempted = 0;

            var broker = new TestServiceBroker ();
            broker.OnNotificationFailed += (notification, exception) => {
                failed++;
            };
            broker.OnNotificationSucceeded += (notification) => {
                succeeded++;  
            };
			broker.Start ();
			broker.ChangeScale (1);

            var c = Log.StartCounter ();

            for (int i = 1; i <= 1000; i++) {
                attempted++;
                broker.QueueNotification (new TestNotification { TestId = i });
            }

			broker.Stop ();
		
            c.StopAndLog ("Test Took {0} ms");

            Assert.AreEqual (attempted, succeeded);
            Assert.AreEqual (0, failed);
		}


        [Test]
        #pragma warning disable 1998
        public async Task Broker_Some_Fail ()
        #pragma warning restore 1998
        {
            var succeeded = 0;
            var failed = 0;
            var attempted = 0;

            const int count = 10;
            var failIds = new [] { 3, 5, 7 };

            var broker = new TestServiceBroker ();
            broker.OnNotificationFailed += (notification, exception) =>
                failed++;
            broker.OnNotificationSucceeded += (notification) =>
                succeeded++;

            broker.Start ();
            broker.ChangeScale (1);

            var c = Log.StartCounter ();

            for (int i = 1; i <= count; i++) {
                attempted++;
                broker.QueueNotification (new TestNotification {
                    TestId = i,
                    ShouldFail = failIds.Contains (i)
                });
            }

            broker.Stop ();

            c.StopAndLog ("Test Took {0} ms");

            Assert.AreEqual (attempted - failIds.Length, succeeded);
            Assert.AreEqual (failIds.Length, failed);
        }
    }
}

