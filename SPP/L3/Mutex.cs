using System;
using System.Threading;

namespace SPP.L3 {
    public class Mutex {
        private int _mutex = 0;

        public void Lock() {
            while (Interlocked.CompareExchange(ref _mutex, 1, 0) != 0)
                Thread.Yield();
        }

        public void Unlock() {
            _mutex = 0;
        }
    }
}