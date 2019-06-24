
using Cloo;
using OCLSharp;
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
    /// Emulation example
    /// </summary>
    public class EmulationExample : IExample
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
            // Emulator
           // RunEmulator();

            // Actual OpenCL
            RunOpenCL();






        }

        private void RunEmulator()
        {
            Bitmap bitmap = (Bitmap)Bitmap.FromFile("elbird.jpg");
            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            int byteCount = bitmapData.Stride * bitmap.Height;
            byte[] pixels = new byte[byteCount];
            IntPtr ptrFirstPixel = bitmapData.Scan0;
            Marshal.Copy(ptrFirstPixel, pixels, 0, pixels.Length);

            int[] ndRange = new int[] { bitmap.Width, bitmap.Height, 1 };
            int[] workGroupSize = new int[] { 2, 2, 1 };


            var emulator = new Emulator<VectorAddProgram>(workGroupSize, ndRange);
            Stopwatch sw = new Stopwatch();
            sw.Start();


            byte[] result = new byte[pixels.Length];

            // Run emulator
            emulator.Run("Sobel", new object[] { pixels, result, bitmap.Width, bitmap.Height });

            sw.Stop();
            Console.WriteLine($"Emulation took: {sw.ElapsedMilliseconds} ms");

            // Put pixels back in bitmap
            Marshal.Copy(result, 0, ptrFirstPixel, pixels.Length);
            bitmap.UnlockBits(bitmapData);
            bitmap.Save("sobelresemulator.jpg");
        }

        private void RunOpenCL()
        {
            // Translate
            string csCode = File.ReadAllText("Kernels/VectorAddProgram.cs");
            string clCode = new Translator(csCode).Translate();
            File.WriteAllText("Kernels/VectorAddProgram.cl", clCode);
            //string clCode = File.ReadAllText("Kernels/VectorAddProgram.cl");
            return;

            Bitmap bitmap = (Bitmap)Bitmap.FromFile("elbird.jpg");
            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            int byteCount = bitmapData.Stride * bitmap.Height;
            byte[] pixels = new byte[byteCount];
            IntPtr ptrFirstPixel = bitmapData.Scan0;
            Marshal.Copy(ptrFirstPixel, pixels, 0, pixels.Length);

            int width = bitmap.Width;
            int height = bitmap.Height;

            ComputePlatform platform = ComputePlatform.Platforms[0];
            ComputeDevice device = platform.Devices[0];

            // Create a new OpenCL context
            ComputeContextPropertyList properties = new ComputeContextPropertyList(platform);
            ComputeContext context = new ComputeContext(new ComputeDevice[] { device }, properties, null, IntPtr.Zero);

            // Create the arrays and fill them with random data.
            byte[] result = new byte[pixels.Length];

            // Create the input buffers and fill them with data from the arrays.
            // Access modifiers should match those in a kernel.
            // CopyHostPointer means the buffer should be filled with the data provided in the last argument.
            ComputeBuffer<byte> inputMem = new ComputeBuffer<byte>(context, ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.CopyHostPointer, pixels);

            // The output buffer doesn't need any data from the host. Only its size is specified (arrC.Length).
            ComputeBuffer<byte> outputMem = new ComputeBuffer<byte>(context, ComputeMemoryFlags.WriteOnly, pixels.Length);

            // Create and build the opencl program.
            //string clProgramSource = File.ReadAllText("sobel.cl");
            ComputeProgram program;
            program = new ComputeProgram(context, clCode);
            program.Build(null, null, null, IntPtr.Zero);


            // Create the kernel function and set its arguments.
            ComputeKernel kernel = program.CreateKernel("Sobel");
            kernel.SetMemoryArgument(0, inputMem);
            kernel.SetMemoryArgument(1, outputMem);
            kernel.SetValueArgument(2, width);
            kernel.SetValueArgument(3, height);

            // Create the event wait list. An event list is not really needed for this example but it is important to see how it works.
            // Note that events (like everything else) consume OpenCL resources and creating a lot of them may slow down execution.
            // For this reason their use should be avoided if possible.
            ComputeEventList eventList = new ComputeEventList();

            // Create the command queue. This is used to control kernel execution and manage read/write/copy operations.
            ComputeCommandQueue commands = new ComputeCommandQueue(context, context.Devices[0], ComputeCommandQueueFlags.None);

            Stopwatch sw = new Stopwatch();
            sw.Start();


            // Execute the kernel "count" times. After this call returns, "eventList" will contain an event associated with this command.
            // If eventList == null or typeof(eventList) == ReadOnlyCollection<ComputeEventBase>, a new event will not be created.
            commands.Execute(kernel, null, new long[] { width, height }, new long[] { 16, 16 }, eventList);



            // Read back the results. If the command-queue has out-of-order execution enabled (default is off), ReadFromBuffer 
            // will not execute until any previous events in eventList (in our case only eventList[0]) are marked as complete 
            // by OpenCL. By default the command-queue will execute the commands in the same order as they are issued from the host.
            // eventList will contain two events after this method returns.
            commands.ReadFromBuffer(outputMem, ref result, false, eventList);

            // A blocking "ReadFromBuffer" (if 3rd argument is true) will wait for itself and any previous commands
            // in the command queue or eventList to finish execution. Otherwise an explicit wait for all the opencl commands 
            // to finish has to be issued before "arrC" can be used. 
            // This explicit synchronization can be achieved in two ways:

            // 1) Wait for the events in the list to finish,
            //eventList.Wait();

            // 2) Or simply use
            commands.Finish();

            sw.Stop();
            Console.WriteLine($"OpenCL: {sw.ElapsedMilliseconds} ms");



            // cleanup commands
            commands.Dispose();

            // cleanup events
            foreach (ComputeEventBase eventBase in eventList)
            {
                eventBase.Dispose();
            }
            eventList.Clear();

            // cleanup kernel
            kernel.Dispose();

            // cleanup program
            program.Dispose();

            // cleanup buffers
            inputMem.Dispose();
            outputMem.Dispose();


            // Put pixels back in bitmap
            Marshal.Copy(result, 0, ptrFirstPixel, pixels.Length);
            bitmap.UnlockBits(bitmapData);
            bitmap.Save("sobelresgpu.jpg");
        }
    }
}
