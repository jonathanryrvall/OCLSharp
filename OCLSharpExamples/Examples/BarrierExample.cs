using OCLSharp;
using OCLSharpExamples.Kernels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCLSharpExamples.Examples
{
    /// <summary>
    /// Example demonstrating barriers
    /// </summary>
    public class BarrierExample : IExample
    {
        /// <summary>
        /// Returns description for this example
        /// </summary>
        public string Description
        {
            get
            {
                return "Example demonstrating barriers memory";
            }
        }

        /// <summary>
        /// Run example
        /// </summary>
        public void Run()
        {
            Emulate();
        }

        /// <summary>
        /// Emulate barrier example
        /// </summary>
        private void Emulate()
        {
            int[] data = Enumerable.Range(0, 128).ToArray();

            int[] ndRange = new int[] { 128, 1, 1 };
            int[] workGroupSize = new int[] { 8, 1, 1 };


            var emulator = new Emulator<BarrierDemoKernels>(workGroupSize, ndRange);

            // Run emulator
            emulator.Run("ReverseWorkGroupData", new object[] { data });

            // Output to console
            foreach (int d in data)
            {
                Console.Write(d.ToString() + " ");
            }
        }


    }
}
