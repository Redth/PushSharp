using System;
using System.Linq;
using PushSharp.Core;
using System.Threading.Tasks;

namespace PushSharp.Tests
{
    public class TestNotification : INotification
	{
		public static int TESTID = 1;

		public TestNotification()
		{
            TestId = TestNotification.TESTID++;
		}

        public object Tag { get;set; }
		public int TestId  { get;set; }

        public bool ShouldFail { get;set; }

		public override string ToString ()
		{
			return TestId.ToString();
		}

        public bool IsDeviceRegistrationIdValid ()
        {
            return true;
        }
	}

	public class TestServiceConnectionFactory : IServiceConnectionFactory<TestNotification>
	{
		public IServiceConnection<TestNotification> Create ()
		{
			return new TestServiceConnection ();
		}
	}

    public class TestServiceBroker : ServiceBroker<TestNotification>
    {
        public TestServiceBroker (TestServiceConnectionFactory connectionFactory) : base (connectionFactory)
        {
        }

        public TestServiceBroker () : base (new TestServiceConnectionFactory ())
        {
        }
    }

    public class TestServiceConnection : IServiceConnection<TestNotification>
	{		
        public async Task Send (TestNotification notification)
		{         
            var id = notification.TestId;

            await Task.Delay (250).ConfigureAwait (false);

            if (notification.ShouldFail) {
                Console.WriteLine ("Fail {0}...", id);
                throw new Exception ("Notification Should Fail: " + id);
            }
		}
	}
} 

