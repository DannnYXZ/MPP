using System;
using System.Collections.Generic;
using System.IO;
using SPP.L1;
using SPP.L5;

namespace SPP.L2 {
    public static class L2 {
        private static Queue<TaskQueue.TaskDelegate> _taskQueue;

        private static string GetSourceDirPath() {
            Console.WriteLine("Enter source dir location:");
            return Console.ReadLine();
        }

        private static string GetTargetDirPath() {
            Console.WriteLine("Enter target dir location:");
            return Console.ReadLine();
        }

        static void CollectCopyTasks(DirectoryInfo source, DirectoryInfo target) {
            if (source.FullName == target.FullName)
                return;
            if (Directory.Exists(target.FullName) == false)
                Directory.CreateDirectory(target.FullName);
            foreach (FileInfo fi in source.GetFiles()) {
                _taskQueue.Enqueue(() => {
                    Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                    fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
                });
            }
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories()) {
                DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
                CollectCopyTasks(diSourceSubDir, nextTargetSubDir);
            }
        }

        public static void Solve() {
            var source = GetSourceDirPath();
            var target = GetTargetDirPath();
            DirectoryInfo diSource = new DirectoryInfo(source);
            DirectoryInfo diTarget = new DirectoryInfo(target);
            _taskQueue = new Queue<TaskQueue.TaskDelegate>();
            CollectCopyTasks(diSource, diTarget);
            Console.WriteLine("Copied {0} files", Parallel.WaitAll(_taskQueue.ToArray()));
        }
    }
}