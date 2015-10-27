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
	public class Test
	{
		[Test]
        public void TestCase ()
		{
            var broker = new TestServiceBroker ();
			broker.Start ();
			broker.ChangeScale (1);

            var c = Log.StartCounter ();

			for (int i = 1; i <= 1000; i++)
				broker.QueueNotification (new TestNotification { TestId = i });

			Console.WriteLine ("Stopping");
			broker.Stop ();
			Console.WriteLine ("Stopped");

            c.StopAndLog ("Test Took {0} ms");
		}


        [Test]
        public async Task TestSomeFail ()
        {
            const int count = 10;
            var failIds = new [] { 3, 5, 7 };

            var failedIds = new List<int> ();
            var successIds = new List<int> ();

            var broker = new TestServiceBroker ();
            broker.OnNotificationFailed += (notification, exception) =>
                failedIds.Add (notification.TestId);
            broker.OnNotificationSucceeded += (notification) =>
                successIds.Add (notification.TestId);

            broker.Start ();
            broker.ChangeScale (1);

            var c = Log.StartCounter ();

            for (int i = 1; i <= count; i++)
                broker.QueueNotification (new TestNotification { TestId = i, ShouldFail = failIds.Contains (i) });

            Console.WriteLine ("Stopping");
            broker.Stop ();
            Console.WriteLine ("Stopped");

            c.StopAndLog ("Test Took {0} ms");

            Assert.AreEqual (failIds.Length, failedIds.Count, "Failed Counts do not match");
            Assert.AreEqual (count - failIds.Length, successIds.Count, "Success Counts do not match");
        }
    }
}

