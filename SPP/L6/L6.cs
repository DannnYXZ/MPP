using System;

namespace SPP.L6 {
    public static class L6 {
        private static void PrintListStats<T>(DynamicList<T> list) {
            Console.WriteLine("Capacity: {0}", list.Capacity);
            Console.WriteLine("Length: {0}", list.Count);
            Console.WriteLine("Content:");
            foreach (var e in list)
                Console.WriteLine(e);
            Console.WriteLine("");
        }

        public static void Solve() {
            var list = new DynamicList<string>();
            PrintListStats(list);
            list.Add("one");
            list.Add("two");
            list.Add("three");
            list.Add("four");
            list.Add("four");
            list.Add("five");
            PrintListStats(list);
            list.Remove("four");
            PrintListStats(list);
            list.RemoveAt(0);
            PrintListStats(list);
            Console.WriteLine(list.Items[3]);
            list.Clear();
            PrintListStats(list);
        }
    }
}