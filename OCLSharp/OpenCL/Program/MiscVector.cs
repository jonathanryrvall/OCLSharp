using System;
using System.Collections.Generic;
using System.Text;

namespace OCLSharp.OpenCL.Program
{
    /// <summary>
    /// Please download the full method specification from here: https://www.khronos.org/registry/OpenCL/specs/2.2/pdf/OpenCL_C.pdf
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class OpenCLFunctions
    {
        protected T shuffle<T>(T x, dynamic mask) where T : struct
        {
            return x;
        }

        protected T shuffle2<T>(T x, dynamic mask) where T : struct
        {
            return x;
        }

        protected int vec_step<T>(T a) where T : struct
        {
            return 0;
        }
    }
}
