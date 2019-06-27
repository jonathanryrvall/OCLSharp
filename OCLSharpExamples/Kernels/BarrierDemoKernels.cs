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
    /// Some kernels demonstrating the use of barriers
    /// </summary>
    public class BarrierDemoKernels : OpenCLProgram
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

            // Get index to read from and also store the value in temporary variable
            size_t readIndex = globalID - 2 * localID + workGroupSize - 1;
            int value = data[readIndex];

            // Wait for all workitems to reach this point
            barrier(args, CLK_GLOBAL_MEM_FENCE);

            // Set data at localID
            data[globalID] = value;
        }
    }
}
