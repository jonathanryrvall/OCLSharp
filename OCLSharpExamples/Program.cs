using OCLSharpExamples.Examples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCLSharpExamples
{
    class Program
    {
        static void Main(string[] args)
        {
            var examples = ExampleInstances().ToList();


            // List all available examples
            foreach (IExample e in examples)
            {
                int i = examples.IndexOf(e);
                Console.WriteLine($"[{i}] - {e.ToString()} - {e.Description}");
            }

            // Allow user to choose an example
            int exampleIndex = int.Parse(Console.ReadLine());
            IExample example = examples[exampleIndex];
            example.Run();


            // Wait for finish
            Console.WriteLine("Press ENTER to quit...");
            Console.ReadLine();
        }

        /// <summary>
        /// Returns instances of examples
        /// </summary>
        static IEnumerable<IExample> ExampleInstances()
        {
            return AllExamples().Select(e => (IExample)Activator.CreateInstance(e));
        }

        /// <summary>
        /// Returns all example types
        /// </summary>
        static IEnumerable<Type> AllExamples()
        {
            var type = typeof(IExample);
            return AppDomain.CurrentDomain.GetAssemblies()
                                          .SelectMany(s => s.GetTypes())
                                          .Where(p => type.IsAssignableFrom(p) && !p.IsInterface);
        }
    }
}
