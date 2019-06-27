using OCLSharp;
using OCLSharp.Attributes;
using OCLSharp.Emulation;
using OCLSharp.OpenCL.DataTypes.ScalarDataTypes;
using OCLSharp.OpenCL.Program;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCLSharpExamples.Kernels
{

    /// <summary>
    /// Kernels demonstrating the use of local memory
    /// </summary>
    public class LocalMemoryDemoKernels : OpenCLProgram
    {
        /// <summary>
        /// Reverse data within work group
        /// </summary>
        [Kernel]
        public void ReverseWorkGroupData(WorkItemArgs args,
                                         [Global] int[] data)
        {
            // Get workgroup size and local id
            size_t workGroupSize = args.get_local_size(0);
            size_t localID = args.get_local_id(0);
            size_t globalID = args.get_global_id(0);

            
            int[] localMemory = GetLocalMem(args, new int[8], "testTag");

            localMemory[localID] = data[globalID];

            size_t swapIndex = workGroupSize - 1 - localID;


            // Wait for all workitems to reach this point
            barrier(args, CLK_GLOBAL_MEM_FENCE);

            // Set data at localID
            data[globalID] = localMemory[swapIndex];
        }
    }
}
