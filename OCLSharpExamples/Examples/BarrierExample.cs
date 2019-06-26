using Cloo;
using OCLSharp;
using OCLSharpExamples.Kernels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
            // Create OpenCL context
            var context = new ContextGenerator().GetContext();
            Console.Clear();
    
            // Explain what kernel will do
            Console.WriteLine("Kernel will reverse the order of the integers within each workgroup");
            Console.WriteLine();

            // Emulator
            Console.WriteLine("Emulating kernel...");
            Console.WriteLine();
            int timeEmulator = Emulate();
            Console.WriteLine($"Emulation completed in {timeEmulator} ms");


            // OpenCL
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Running in OpenCL...");
            Console.WriteLine();
            int timeOpenCL = RunOpenCL(context);
            Console.WriteLine($"OpenCL execution completed in {timeOpenCL} ms (Not including compilation)");


        }

        /// <summary>
        /// Emulate example
        /// </summary>
        private int Emulate()
        {
            // Create some test data
            int[] data = Enumerable.Range(0, 128).ToArray();

            // Print indata
            Console.WriteLine("Input data:");
            foreach (int d in data)
            {
                Console.Write(d.ToString() + " ");
            }
            Console.Write("\n\n");

            // Set workgroup and total work size
            int[] ndRange = new int[] { 128 };
            int[] workGroupSize = new int[] { 8 };

            // Create a new emulator
            var emulator = new Emulator<BarrierDemoKernels>(workGroupSize, ndRange);

            // Start stopwatch
            Stopwatch sw = new Stopwatch();
            sw.Start();

            // Run emulator
            emulator.Run("ReverseWorkGroupData", new object[] { data });

            sw.Stop();


            // Print result
            Console.WriteLine("Result data:");
            foreach (int d in data)
            {
                Console.Write(d.ToString() + " ");
            }
            Console.Write("\n\n");

            // Finish and return time result
            return (int)sw.ElapsedMilliseconds;
           
        }

        /// <summary>
        /// Run example in OpenCL
        /// </summary>
        private int RunOpenCL(ComputeContext context)
        {
            // Create some test data
            int[] data = Enumerable.Range(0, 128).ToArray();

            // Print indata
            Console.WriteLine("Input data:");
            foreach (int d in data)
            {
                Console.Write(d.ToString() + " ");
            }
            Console.Write("\n\n");

            // Tranlate CS to OpenCL
            string csCode = File.ReadAllText("Kernels/BarrierDemoKernels.cs");
            string clCode = new OCLSharp.Translating.Translator(csCode).Translate();
            File.WriteAllText("Kernels/BarrierDemoKernels.cl", clCode);


          

           
            // Create and build the opencl program.
            ComputeProgram program = new ComputeProgram(context, clCode);
            program.Build(null, null, null, IntPtr.Zero);

            Stopwatch sw = new Stopwatch();
            sw.Start();

            // Create the input buffers and fill them with data from the arrays.
            ComputeBuffer<int> mem = new ComputeBuffer<int>(context, ComputeMemoryFlags.ReadWrite | ComputeMemoryFlags.CopyHostPointer, data);

            // Create the kernel function and set its arguments.
            ComputeKernel kernel = program.CreateKernel("ReverseWorkGroupData");
            kernel.SetMemoryArgument(0, mem);

            // Create the command queue. This is used to control kernel execution and manage read/write/copy operations.
            ComputeCommandQueue commands = new ComputeCommandQueue(context, context.Devices[0], ComputeCommandQueueFlags.None);

            // Execute the kernel "count" times. After this call returns, "eventList" will contain an event associated with this command.
            commands.Execute(kernel, null, new long[] { data.Length }, new long[] { 8 }, null);

            // Read back the results
            commands.ReadFromBuffer(mem, ref data, false, null);

            // Finish!
            commands.Finish();

            sw.Stop();

            // cleanup
            commands.Dispose();
            kernel.Dispose();
            program.Dispose();
            mem.Dispose();

            // Print result
            Console.WriteLine("Result data:");
            foreach (int d in data)
            {
                Console.Write(d.ToString() + " ");
            }
            Console.Write("\n\n");

            return (int)sw.ElapsedMilliseconds;
        }


    }
}
