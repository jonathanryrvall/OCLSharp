using OCLSharp.OpenCL.DataTypes.ScalarDataTypes;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace OCLSharp.OpenCL.DataTypes.VectorDataTypes
{
    /// <summary>
    /// Generic 3-component vector
    /// </summary>
    public class Vector2<T> : Vector<T>
    {
        public T x, y;
    }

    /// <summary>
    /// A 2-Component signed char vector
    /// </summary>
    public class char2 : Vector2<sbyte> { }

    /// <summary>
    /// A 2-Component unsigned char vector
    /// </summary>
    public class uchar2 : Vector2<byte> { }

    /// <summary>
    /// A 2-Component signed 16 bit integer
    /// </summary>
    public class short2 : Vector2<short> { }

    /// <summary>
    /// A 2-Component unsigned 16 bit integer
    /// </summary>
    public class ushort2 : Vector2<ushort> { }

    /// <summary>
    /// A 2-Component signed 32 bit integer
    /// </summary>
    public class int2 : Vector2<int> { }

    /// <summary>
    /// A 2-Component unsigned 32 bit integer
    /// </summary>
    public class uint2 : Vector2<uint> { }

    /// <summary>
    /// A 2-Component signed 64 bit integer
    /// </summary>
    public class long2 : Vector2<long> { }

    /// <summary>
    /// A 2-Component unsigned 64 bit integer
    /// </summary>
    public class ulong2 : Vector2<ulong> { }

    /// <summary>
    /// A 2-Component unsigned 32 bit floating point
    /// </summary>
    public class float2 : Vector2<float> { }

    /// <summary>
    /// A 2-Component unsigned 64 bit floating point 
    /// </summary>
    public class double2 : Vector2<double> { }



}
