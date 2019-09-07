using System;
using System.Linq;
using System.Reflection;

namespace SPP.L4 {
    public static class L4 {
        static string GetAssemblyPath() {
            Console.WriteLine("Enter assembly location:");
            return Console.ReadLine();
        }

        public static void Solve() {
            var assemblyPath = GetAssemblyPath();
            var assembly = Assembly.LoadFile(assemblyPath);
            var types = assembly.GetTypes();
            var sortedTypes = types.OrderBy(x => x.Namespace).ThenBy(x => x.Name);
            foreach (var type in sortedTypes)
                if (type.IsPublic)
                    Console.WriteLine(type.FullName);
        }
    }
}