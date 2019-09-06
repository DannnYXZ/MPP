using System.Collections.Generic;
using System.Threading;

namespace SPP.L1 {
    public class PCTaskQueue {
        public delegate void TaskDelegate();

        private static Queue<TaskDelegate> _taskQueue;
        readonly Thread[] threads;

        private Mutex mutex = new Mutex();
        Semaphore fillCnt = new Semaphore(0, 1000);

        public PCTaskQueue(int maxThreads) {
            _taskQueue = new Queue<TaskDelegate>();
            threads = new Thread[maxThreads];
            for (int i = 0; i < threads.Length; i++) {
                threads[i] = new Thread(() => {
                    while (true) {
                        fillCnt.WaitOne();
                        mutex.WaitOne();
                        var task = _taskQueue.Dequeue();
                        mutex.ReleaseMutex();
                        task();
                    }
                });
                threads[i].IsBackground = true;
                threads[i].Start();
            }
        }

        public void EnqueueTask(TaskDelegate task) {
            mutex.WaitOne();
            _taskQueue.Enqueue(task);
            mutex.ReleaseMutex();
            fillCnt.Release(1);
        }
    }
}