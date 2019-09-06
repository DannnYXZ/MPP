using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;

namespace SPP.L2 {
    public static class L2 {
        delegate void TaskDelegate();

        private static BlockingCollection<TaskDelegate> _taskQueue;
        private static Thread[] threads;
        private static int tasksFailed = 0;

        private static string GetSourceDirPath() {
            Console.WriteLine("Enter source dir location:");
            return Console.ReadLine();
        }

        private static string GetTargetDirPath() {
            Console.WriteLine("Enter target dir location:");
            return Console.ReadLine();
        }

        static void EnqueueCopyTasks(DirectoryInfo source, DirectoryInfo target) {
            if (source.FullName == target.FullName)
                return;
            if (Directory.Exists(target.FullName) == false)
                Directory.CreateDirectory(target.FullName);
            foreach (FileInfo fi in source.GetFiles()) {
                _taskQueue.Add(() => {
                    Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                    fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
                });
            }
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories()) {
                DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
                EnqueueCopyTasks(diSourceSubDir, nextTargetSubDir);
            }
        }

        static void RunThreadPool(int maxThreads) {
            threads = new Thread[maxThreads];
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
        }

        public static void Solve() {
            var source = GetSourceDirPath();
            var target = GetTargetDirPath();
            DirectoryInfo diSource = new DirectoryInfo(source);
            DirectoryInfo diTarget = new DirectoryInfo(target);
            _taskQueue = new BlockingCollection<TaskDelegate>();
            EnqueueCopyTasks(diSource, diTarget);
            int tasksQueued = _taskQueue.Count;
            RunThreadPool(100);
            Console.WriteLine("Copied {0} files", tasksQueued - tasksFailed);
        }
    }
}