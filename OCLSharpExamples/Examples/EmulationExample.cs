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
    /// Emulation example
    /// </summary>
    class EmulationExample : IExample
    {
        /// <summary>
        /// Returns description for this example
        /// </summary>
        public string Description
        {
            get
            {
                return "Example demonstrating emulating the cs code as if it was OpenCL";
            }
        }

        /// <summary>
        /// Run example
        /// </summary>
        public void Run()
        {
            int[] a = Enumerable.Range(0, 128).ToArray();
            int[] b = Enumerable.Repeat(10, 128).ToArray();
            int[] c = new int[128];

            int[] ndRange = new int[] { 128, 1, 1 };
            int[] workGroupSize = new int[] { 1, 1, 1 };


            var emulator = new Emulator<VectorAddProgram>(workGroupSize, ndRange);

            // Run emulator
            emulator.Run("VectorAdd",new object[] { a, b, c });

        }
    }
}
