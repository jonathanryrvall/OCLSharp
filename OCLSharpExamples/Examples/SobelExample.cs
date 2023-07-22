using Cloo;
using OCLSharp;
using OCLSharp.Emulation;
using OCLSharpExamples.Kernels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OCLSharpExamples.Examples
{
    /// <summary>
    /// Simple sobel example
    /// </summary>
    public class SobelExample : IExample
    {
        /// <summary>
        /// Returns description for this example
        /// </summary>
        public string Description
        {
            get
            {
                return "Very simple example demonstrating a sobel kernel written in CSharp emulated as if it was OpenCL";
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
        private int Emulate()
        {
            // Read CS file
            string csCode = File.ReadAllText("Kernels/SobelKernels.cs");

            // Convert to OpenCL
            string clCode = new OCLSharp.Translating.Translator(csCode).Translate();

            // Save OpenCL to file
            File.WriteAllText("Kernels/SobelKernels.cl", clCode);

            // Load sample image
            Bitmap bitmap = (Bitmap)Bitmap.FromFile("Images/tucan.png");
            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            int byteCount = bitmapData.Stride * bitmap.Height;
            byte[] pixels = new byte[byteCount];
            IntPtr ptrFirstPixel = bitmapData.Scan0;
            Marshal.Copy(ptrFirstPixel, pixels, 0, pixels.Length);


            // Emulate kernel
            int[] ndRange = new int[] { bitmap.Width, bitmap.Height, 1 };

            // A much smaller work group size is used, otherwise CPU would only be doing loads of context switching
            int[] workGroupSize = new int[] { 2, 2, 1 };

            var emulator = new Emulator<SobelKernels>(workGroupSize, ndRange);

            byte[] result = new byte[pixels.Length];


            // Run emulator
            emulator.Run("SobelSimple", new object[] { pixels, result, bitmap.Width, bitmap.Height });


            // Put pixels back in bitmap
            Marshal.Copy(result, 0, ptrFirstPixel, pixels.Length);
            bitmap.UnlockBits(bitmapData);
            bitmap.Save("Images/tucan-sobel-emulator.png");
        }

    



        /// <summary>
        /// Run example in OpenCL
        /// </summary>
        private int RunOpenCL(ComputeContext context)
        {

            // Load sample image
            Bitmap bitmap = (Bitmap)Bitmap.FromFile("Images/tucan.png");
            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            int byteCount = bitmapData.Stride * bitmap.Height;
            byte[] pixels = new byte[byteCount];
            IntPtr ptrFirstPixel = bitmapData.Scan0;
            Marshal.Copy(ptrFirstPixel, pixels, 0, pixels.Length);

            int width = bitmap.Width;
            int height = bitmap.Height;

        
            // Tranlate CS to OpenCL
            string csCode = File.ReadAllText("Kernels/SobelKernels.cs");
            string clCode = new OCLSharp.Translating.Translator(csCode).Translate();
            File.WriteAllText("Kernels/SobelKernels.cl", clCode);



            byte[] result = new byte[pixels.Length];

            // Create and build the opencl program.
            ComputeProgram program = new ComputeProgram(context, clCode);
            program.Build(null, null, null, IntPtr.Zero);

            Stopwatch sw = new Stopwatch();
            sw.Start();

            // Create the input buffers and fill them with data from the arrays.
            ComputeBuffer<byte> inputData = new ComputeBuffer<byte>(context, ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.CopyHostPointer, pixels);
            ComputeBuffer<byte> outputData = new ComputeBuffer<byte>(context, ComputeMemoryFlags.WriteOnly, pixels.Length);

            // Create the kernel function and set its arguments.
            ComputeKernel kernel = program.CreateKernel("ReverseWorkGroupData");
            kernel.SetMemoryArgument(0, inputData);
            kernel.SetMemoryArgument(1, outputData);
            kernel.SetValueArgument(2, width);
            kernel.SetValueArgument(3, height);

            // Create the command queue. This is used to control kernel execution and manage read/write/copy operations.
            ComputeCommandQueue commands = new ComputeCommandQueue(context, context.Devices[0], ComputeCommandQueueFlags.None);

            // Execute the kernel "count" times. After this call returns, "eventList" will contain an event associated with this command.
            commands.Execute(kernel, null, new long[] { width, height }, new long[] { 16, 16 }, null);

            // Read back the results
            commands.ReadFromBuffer(outputData, ref result, false, null);

            // Finish!
            commands.Finish();

            sw.Stop();

            // cleanup
            commands.Dispose();
            kernel.Dispose();
            program.Dispose();
            inputData.Dispose();
            outputData.Dispose();

            // Put pixels back in bitmap
            Marshal.Copy(result, 0, ptrFirstPixel, pixels.Length);
            bitmap.UnlockBits(bitmapData);
            bitmap.Save("Images/tucan-sobel-opencl.png");

            return (int)sw.ElapsedMilliseconds;
        }

    }
}