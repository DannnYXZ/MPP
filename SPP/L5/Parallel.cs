using System;
using System.Collections.Concurrent;
using System.Threading;
using SPP.L1;

namespace SPP.L5 {
    public class Parallel {
        private static BlockingCollection<TaskQueue.TaskDelegate> _taskQueue;

        public static int WaitAll(TaskQueue.TaskDelegate[] tasks, int maxThreads = 5) {
            var tasksFailed = 0;
            var tasksQueued = tasks.Length;
            _taskQueue = new BlockingCollection<TaskQueue.TaskDelegate>();
            foreach (var task in tasks)
                _taskQueue.Add(task);
            var threads = new Thread[maxThreads];
            for (int i = 0; i < threads.Length; i++) {
                threads[i] = new Thread(() => {
                    while (_taskQueue.Count > 0) {
                        var task = _taskQueue.Take();
                        try {
                            task();
                        }
                        catch (Exception e) {
                            Interlocked.Increment(ref tasksFailed);
                            Console.WriteLine(e.Message);
                        }
                    }
                });
                threads[i].IsBackground = true;
                threads[i].Start();
                threads[i].Join();
            }
            return tasksQueued - tasksFailed;
        }
    }
}