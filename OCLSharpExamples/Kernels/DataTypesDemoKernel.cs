using OCLSharp;
using OCLSharp.Attributes;
using OCLSharp.Emulation;
using OCLSharp.OpenCL.DataTypes.ScalarDataTypes;
using OCLSharp.OpenCL.DataTypes.VectorDataTypes;
using OCLSharp.OpenCL.Program;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCLSharpExamples.Kernels
{
    /// <summary>
    /// Kernel demonstrating the use of
    /// </summary>
    public class DataTypesDemoKernel : OpenCLProgram
    {
        /// <summary>
        /// Reverse data within work group
        /// </summary>
        [Kernel]
        public void DataTypesTest(WorkItemArgs args,
                                         [Global] [ReadWrite] int[] data)
        {
            size_t globalID = args.get_global_id(0);

            int2 i2 = new int2();

            i2.x = 1;
            i2.y = 1;

            data[globalID] = i2.x + i2.y;


        }
    }
}
