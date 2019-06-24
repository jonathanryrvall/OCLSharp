using OCLSharp;
using OCLSharp.Attributes;
using OCLSharp.OpenCL.Program;
using System;
using System.Collections.Generic;
using System.Text;

namespace OCLSharpExamples.Kernels
{
    public class VectorAddProgram : OpenCLProgram
    {
     
        ///// <summary>
        ///// Test of local memory
        ///// </summary>
        //[Kernel]
        //public void ReverseTest(WorkItemArgs args, [Global] int[] a)
        //{
        //  //  int[] b = GetLocalMem(args, new int[10], "b");
            
        //    //int i = args.GetGlobalID(0);
        //    //c[i] = a[i] + b[i];
        //}

        ///// <summary>
        ///// Perform vector addition
        ///// </summary>
        //[Kernel]
        //public void VectorAdd(WorkItemArgs args,
        //                               [Global] int[] a,
        //                               [Global] int[] b,
        //                               [Global] int[] c)
        //{
        //    int i = args.get_global_id(0);
        //    c[i] = a[i] + b[i];
        //}

        [NonKernel] int GetGlobalIndex(int x, int y, int w)
        {
            return ((y * w) + x) * 3;
        }

        [Constant] int[] sobelMatrix = { -2, -1, 0, 1 , 2,
                    -2, -1, 0, 1 , 2,
                    -4, -2, 0, 2 , 4,
                    -2, -1, 0, 1 , 2,
                    -2, -1, 0, 1 , 2 };

        /// <summary>
        /// Perform vector addition
        /// </summary>
        [Kernel]
        public void Sobel(WorkItemArgs args,
                                       [Global] byte[] inputMem,
                                       [Global] byte[] outputMem,
                                       int width,
                                       int height)
        {
            // Get ids
            int x = args.get_global_id(0);
            int y = args.get_global_id(1);

            // Index 
            int inputIndex = (y * width * 3) + (x * 3);

            // Results
            int resBlue = 0;
            int resGreen = 0;
            int resRed = 0;


            // Iterate through kernel
            for (int ky = 0; ky < 5; ky++)
            {
                // Get y index to sample from
                int gy = y + ky - 2;

                // Index is out of range!
                if (gy < 0 || gy >= height)
                {
                    continue;
                }

                for (int kx = 0; kx < 5; kx++)
                {
                    // Get x index to sample from
                    int gx = x + kx - 2;

                    // Index is out of range!
                    if (gx < 0 || gx >= width)
                    {
                        continue;
                    }

                    // Get index on image to sample from
                    int gi = GetGlobalIndex(gx, gy, width);

                    // Index in sobel kernel
                    int matrixIndex = (ky * 5) + kx;
                    int sobelMultiplier = sobelMatrix[matrixIndex];

                    int sampleBlue = inputMem[gi + 0];
                    int sampleGreen = inputMem[gi + 1];
                    int sampleRed = inputMem[gi + 2];


                    resBlue += sampleBlue * sobelMultiplier;
                    resGreen += sampleGreen * sobelMultiplier;
                    resRed += sampleRed * sobelMultiplier;

                }

            }

            // Clamp
            if (resBlue > 255) resBlue = 255;
            if (resBlue < 0) resBlue = 0;
            if (resGreen > 255) resGreen = 255;
            if (resGreen < 0) resGreen = 0;
            if (resRed > 255) resRed = 255;
            if (resRed < 0) resRed = 0;


            // calculate new pixel value
            outputMem[inputIndex + 0] = (byte)resBlue;
            outputMem[inputIndex + 1] = (byte)resGreen;
            outputMem[inputIndex + 2] = (byte)resRed;


           // barrier(CLK_LOCAL_MEM_FENCE);
        }

        ///// <summary>
        ///// Perform vector addition
        ///// </summary>
        //[Kernel]
        //public void VectorAdd2D(WorkItemArgs args,
        //                               [Global] int[] a,
        //                               [Global] int[] b,
        //                               [Global] int[] c)
        //{
        //    int x = args.get_global_id(0);
        //    int y = args.get_global_id(1);
        //    int w = args.get_global_size(0);
        //    int i = (y * w) + x;

        //    c[i] = a[i] + b[i];
        //}

        ///// <summary>
        ///// Perform vector addition
        ///// </summary>
        //[Kernel]
        //public void VectorSub(WorkItemArgs args,
        //                                [Global] int[] a,
        //                               [Global] int[] b,
        //                               [Global] int[] c)
        //{
        //    int i = args.get_global_id(0);
        //    c[i] = a[i] - b[i];
            
        //}

        ///// <summary>
        ///// Perform vector addition
        ///// </summary>
        //[NonKernel]
        //public int TestMethod(int a, int b, int c)
        //{
        //    return a + b + c;
        //}
    }
}
