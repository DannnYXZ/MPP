using System;
using System.Threading;

namespace SPP.L1 {
    public static class L1 {
        private static void Task1() {
            Console.WriteLine("T1 done");
        }

        private static void Task2() {
            Thread.Sleep(2000);
            Console.WriteLine("T2 done");
        }

        private static void Task3() {
            Console.WriteLine("T3 done");
        }

        static void Task4() {
            Console.WriteLine("T4 done");
        }

        static void Task5() {
            Console.WriteLine("T5 done");
        }

        public static void Solve() {
            TaskQueue tq = new TaskQueue(2);
            tq.EnqueueTask(Task1);
            tq.EnqueueTask(Task2);
            tq.EnqueueTask(Task3);
            tq.EnqueueTask(Task4);
            tq.EnqueueTask(Task5);
            Thread.Sleep(1000);

            PCTaskQueue pq = new PCTaskQueue(2);
            pq.EnqueueTask(Task1);
            pq.EnqueueTask(Task2);
            pq.EnqueueTask(Task3);
            pq.EnqueueTask(Task4);
            pq.EnqueueTask(Task5);
            Thread.Sleep(3000);
        }
    }
}