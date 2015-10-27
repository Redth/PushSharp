using System;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace PushSharp.Tests
{
    public sealed class SocketAwaitable : INotifyCompletion
    { 
        private readonly static Action SENTINEL = () => { };

        internal bool m_wasCompleted; 
        internal Action m_continuation; 
        internal SocketAsyncEventArgs m_eventArgs;

        public SocketAwaitable(SocketAsyncEventArgs eventArgs) 
        { 
            if (eventArgs == null) throw new ArgumentNullException("eventArgs"); 
            m_eventArgs = eventArgs; 
            eventArgs.Completed += delegate 
            { 
                var prev = m_continuation ?? Interlocked.CompareExchange(
                    ref m_continuation, SENTINEL, null); 
                if (prev != null) prev(); 
            }; 
        }

        internal void Reset() 
        { 
            m_wasCompleted = false; 
            m_continuation = null; 
        }

        public SocketAwaitable GetAwaiter() { return this; }

        public bool IsCompleted { get { return m_wasCompleted; } }

        public void OnCompleted(Action continuation) 
        { 
            if (m_continuation == SENTINEL || 
                Interlocked.CompareExchange(
                    ref m_continuation, continuation, null) == SENTINEL) 
            { 
                Task.Run(continuation); 
            } 
        }

        public void GetResult() 
        { 
            if (m_eventArgs.SocketError != SocketError.Success) 
                throw new SocketException((int)m_eventArgs.SocketError); 
        } 
    }

    public static class SocketExtensions 
    { 
        public static SocketAwaitable ReceiveAsync(this Socket socket, 
            SocketAwaitable awaitable) 
        { 
            awaitable.Reset(); 
            if (!socket.ReceiveAsync(awaitable.m_eventArgs)) 
                awaitable.m_wasCompleted = true; 
            return awaitable; 
        }

        public static SocketAwaitable SendAsync(this Socket socket, 
            SocketAwaitable awaitable) 
        { 
            awaitable.Reset(); 
            if (!socket.SendAsync(awaitable.m_eventArgs)) 
                awaitable.m_wasCompleted = true; 
            return awaitable; 
        }

    }


}

