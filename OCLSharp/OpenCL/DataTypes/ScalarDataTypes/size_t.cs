using System;
using System.Collections.Generic;
using System.Text;

namespace OCLSharp.OpenCL.DataTypes.ScalarDataTypes
{
    /// <summary>
    /// size_t struct, uses a 32 bit float for emulation purporses
    /// </summary>
    public struct size_t
    {
        public int Value;

        public static size_t operator +(size_t a, size_t b)
        {
            return new size_t() { Value = a.Value + b.Value };
        }

        public static size_t operator -(size_t a, size_t b)
        {
            return new size_t() { Value = a.Value - b.Value };
        }

        public static size_t operator *(size_t a, size_t b)
        {
            return new size_t() { Value = a.Value * b.Value };
        }

        public static size_t operator /(size_t a, size_t b)
        {
            return new size_t() { Value = a.Value * b.Value };
        }

    }


}
