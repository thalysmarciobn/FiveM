using CitizenFX.Core;
using Server.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Server.Instances
{
    public class ThreadInstance : AbstractInstance<ThreadInstance>
    {
        private readonly List<Thread> _threads = new List<Thread>();
        public Thread CreateThread(Action action)
        {
            var thread = new Thread(() => action()) { IsBackground = true };
            _threads.Add(thread);
            return thread;
        }

        public void Shutdown()
        {
            foreach (var thread in _threads)
            {
                Debug.WriteLine($"[ThreadInstance] Shutdown: {thread.ManagedThreadId}");
                thread.Abort();
            }
        }
    }
}
