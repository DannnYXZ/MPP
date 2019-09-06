using System.Collections.Concurrent;
using System.Threading;

namespace SPP.L1 {
    public class TaskQueue {
        public delegate void TaskDelegate();

        private static BlockingCollection<TaskDelegate> _taskQueue;
        private Thread[] threads;

        public TaskQueue(int maxThreads) {
            _taskQueue = new BlockingCollection<TaskDelegate>();
            threads = new Thread[maxThreads];
            for (int i = 0; i < threads.Length; i++) {
                threads[i] = new Thread(() => {
                    while (true) {
                        var task = _taskQueue.Take();
                        task();
                    }
                });
                threads[i].IsBackground = true;
                threads[i].Start();
            }
        }

        public void EnqueueTask(TaskDelegate task) {
            _taskQueue.Add(task);
        }
    }
}