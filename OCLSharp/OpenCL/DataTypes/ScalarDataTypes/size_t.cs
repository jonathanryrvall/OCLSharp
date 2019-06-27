using System;
using System.Collections.Generic;
using System.Text;

namespace OCLSharp.OpenCL.DataTypes.ScalarDataTypes
{
    /// <summary>
    /// size_t struct
    /// </summary>
    public struct size_t
    {
        public uint Value;

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


        public static implicit operator size_t(uint value)
        {
            return new size_t { Value = value };
        }

        public static implicit operator size_t(int value)
        {
            return new size_t { Value = (uint)value };
        }

        public static implicit operator uint(size_t s)
        {
            return s.Value;
        }

        public static implicit operator int(size_t s)
        {
            return (int)s.Value; ;
        }

    }


}
