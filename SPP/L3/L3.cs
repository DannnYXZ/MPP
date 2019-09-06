using System;
using System.Threading;
using System.Xml;

namespace SPP.L3 {
    public static class L3 {
        private static Mutex mutex = new Mutex();

        public static void Solve() {
            var rand = new Random();
            for (int i = 0; i < 30; i++) {
                var ci = i;
                var thread = new Thread(() => {
                    Console.WriteLine("Thread {0} started", ci);
                    mutex.Lock();
                    Console.WriteLine("Thread {0} locked mutex", ci);
                    Thread.Sleep(rand.Next(10, 100));
                    Console.WriteLine("Thread {0} unlocked mutex", ci);
                    mutex.Unlock();
                });
                thread.Start();
            }
        }
    }
}