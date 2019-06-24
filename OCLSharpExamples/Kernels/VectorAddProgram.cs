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
     
        /// <summary>
        /// Test of local memory
        /// </summary>
        [Kernel]
        public void ReverseTest(WorkItemArgs args, [Global] int[] a)
        {
          //  int[] b = GetLocalMem(args, new int[10], "b");
            
            //int i = args.GetGlobalID(0);
            //c[i] = a[i] + b[i];
        }

        /// <summary>
        /// Perform vector addition
        /// </summary>
        [Kernel]
        public void VectorAdd(WorkItemArgs args,
                                       [Global] int[] a,
                                       [Global] int[] b,
                                       [Global] int[] c)
        {
            int i = args.get_global_id(0);
            c[i] = a[i] + b[i];
        }

        /// <summary>
        /// Perform vector addition
        /// </summary>
        [Kernel]
        public void VectorSub(WorkItemArgs args,
                                        [Global] int[] a,
                                       [Global] int[] b,
                                       [Global] int[] c)
        {
            int i = args.get_global_id(0);
            c[i] = a[i] - b[i];
            
        }

        /// <summary>
        /// Perform vector addition
        /// </summary>
        [NonKernel]
        public int TestMethod(int a, int b, int c)
        {
            return a + b + c;
        }
    }
}
