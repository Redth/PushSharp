using System;

namespace PushSharp.Tests.Runner
{
    class MainClass
    {
        public static void Main (string[] args)
        {
            Run ();
            Console.ReadLine ();
        }

        public static async void Run ()
        {
            Console.WriteLine ("Running TestCase");

            var test = new GcmXmppTests ();
            await test.GCMXMPP_Connect ().ConfigureAwait (false);

            Console.WriteLine ("Done Running TestCase");

        }
    }
}
