using System;
using System.Collections.Generic;
using System.Threading;

namespace NzbDrone.Test.Common
{
    public class ConcurrencyCounter
    {
        private int _items;
        readonly object _mutex = new object();
        readonly Dictionary<int, int> _threads = new Dictionary<int, int>();

        public int MaxThreads { get { return _threads.Count; } }

        public ConcurrencyCounter(int items)
        {
            _items = items;
        }

        public void WaitForAllItems()
        {
            while (_items != 0)
            {
                Thread.Sleep(500);
            }
        }

        public int Start()
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            lock (_mutex)
            {

                _threads[threadId] = 1;
            }

            Console.WriteLine("Starting " + threadId);
            return threadId;
        }

        public void SimulateWork(int sleepInMs)
        {
            var id = Start();
            Thread.Sleep(sleepInMs);
            Stop(id);
        }

        public void Stop(int id)
        {
            Console.WriteLine("Finished " + id);
            lock (_mutex)
            {
                _items--;
            }
        }
    }
}