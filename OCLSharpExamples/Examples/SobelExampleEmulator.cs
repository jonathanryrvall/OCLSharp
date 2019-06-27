using Cloo;
using OCLSharp;
using OCLSharp.Emulation;
using OCLSharpExamples.Kernels;
using System;
using System.Collections.Generic;
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
    public class SobelExampleEmulator : IExample
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

     
    }
}