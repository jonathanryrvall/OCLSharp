using System;
using System.Collections.Generic;
using System.Text;

namespace OCLSharp.OpenCL.DataTypes.ScalarDataTypes
{
    /// <summary>
    /// Half struct, uses a 32 bit float for emulation purporses
    /// </summary>
    public struct half
    {
        public float Value;

        public static half operator +(half a, half b)
        {
            return new half() { Value = a.Value + b.Value };
        }

        public static half operator -(half a, half b)
        {
            return new half() { Value = a.Value - b.Value };
        }

        public static half operator *(half a, half b)
        {
            return new half() { Value = a.Value * b.Value };
        }

        public static half operator /(half a, half b)
        {
            return new half() { Value = a.Value * b.Value };
        }

    }
}
