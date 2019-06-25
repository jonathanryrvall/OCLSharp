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
                return "Example demonstrating local memory";
            }
        }

        /// <summary>
        /// Run example
        /// </summary>
        public void Run()
        {
            string csCode = File.ReadAllText("Kernels/VectorAddProgram.cs");
            string openCLCode = new OCLSharp.Translating.Translator(csCode).Translate();
            File.WriteAllText("Kernels/VectorAddProgram.cl", openCLCode);
        }
    }
}
