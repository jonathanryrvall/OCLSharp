using Cloo;
using OCLSharp;
using OCLSharpExamples.Kernels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCLSharpExamples.Examples
{

    /// <summary>
    /// Example demontrating local memory
    /// </summary>
    public class LocalMemoryExample : IExample
    {
        /// <summary>
        /// Returns description for this example
        /// </summary>
        public string Description
        {
            get
            {
                return "Example demonstrating the use of local memory";
            }
        }

        /// <summary>
        /// Run example
        /// </summary>
        public void Run()
        {
            int[] inputData = Enumerable.Range(0, 128).ToArray();


            // Run emulation
            Emulate(inputData);


            var context = new ContextGenerator().GetContext();

            // Read CS code from file
            string csCode = File.ReadAllText("Kernels/LocalMemoryDemoKernels.cs");
            string clCode = new OCLSharp.Translating.Translator(csCode).Translate();
            File.WriteAllText("Kernels/LocalMemoryDemoKernels.cl", clCode);
      

        }

        /// <summary>
        /// Emulate barrier example
        /// </summary>
        private void Emulate(int[] inputData)
        {
            int[] data = inputData.Clone() as int[];

            int[] ndRange = new int[] { 128, 1, 1 };
            int[] workGroupSize = new int[] { 8, 1, 1 };


            var emulator = new Emulator<LocalMemoryDemoKernels>(workGroupSize, ndRange);

            // Run emulator
            emulator.Run("ReverseWorkGroupData", new object[] { inputData });


            Console.WriteLine("Emulation result:");
            Console.WriteLine();
            // Output to console
            foreach (int d in inputData)
            {
                Console.Write(d.ToString() + " ");
            }
        }


      

    }
}
