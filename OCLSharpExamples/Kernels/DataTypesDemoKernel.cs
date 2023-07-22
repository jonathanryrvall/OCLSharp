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

            float3 threeComponentsA = new float3();
            threeComponentsA.x = 5;
            threeComponentsA.y = 6;
            threeComponentsA.z = 7;

            float3 threeComponentsB = new float3();
            threeComponentsB.x = 30.4f;
            threeComponentsB.y = 40.3f;
            threeComponentsB.z = 50.2f;

            float3 threeComponentsC = threeComponentsA + threeComponentsB;

            //int2 i2 = new int2();
            //int2 i2a = new int2();
            //i2a.x = 5;
            //i2a.y = 7;




            //i2.x = 1;
            //i2.y = 1;
            //int2 i2c = i2 + i2a;


            data[globalID] = 0;


        }
    }
}
