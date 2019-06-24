using OCLSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OCLSharpExamples.Examples
{
    /// <summary>
    /// Example demonstrating a simple translation of code
    /// </summary>
    public class Example1 : IExample
    {
        /// <summary>
        /// Returns description for this example
        /// </summary>
        public string Description
        {
            get
            {
                return "Example demonstrating a simple translation of code";
            }
        }

        /// <summary>
        /// Run example
        /// </summary>
        public void Run()
        {
            string csCode = File.ReadAllText("Kernels/VectorAddProgram.cs");
            string openCLCode = new Translator(csCode).Translate();
            File.WriteAllText("Kernels/VectorAddProgram.cl", openCLCode);
        }
    }
}
