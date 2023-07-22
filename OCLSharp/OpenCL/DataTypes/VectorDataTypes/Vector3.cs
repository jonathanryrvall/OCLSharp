using System;
using System.Collections.Generic;
using System.Text;

namespace OCLSharp.OpenCL.DataTypes.VectorDataTypes
{
    /// <summary>
    /// Generic 3-component vector
    /// </summary>
    public class Vector3<T> : Vector<T>
    {
        public T x, y, z;
    }

    /// <summary>
    /// A 3-Component signed char vector
    /// </summary>
    public class char3 : Vector3<sbyte> { }

    /// <summary>
    /// A 3-Component unsigned char vector
    /// </summary>
    public class uchar3 : Vector3<byte> { }

    /// <summary>
    /// A 3-Component signed 16 bit integer
    /// </summary>
    public class short3 : Vector3<short> { }

    /// <summary>
    /// A 3-Component unsigned 16 bit integer
    /// </summary>
    public class ushort3 : Vector3<ushort> { }

    /// <summary>
    /// A 3-Component signed 32 bit integer
    /// </summary>
    public class int3 : Vector3<int> { }

    /// <summary>
    /// A 3-Component unsigned 32 bit integer
    /// </summary>
    public class uint3 : Vector3<uint> { }

    /// <summary>
    /// A 3-Component signed 64 bit integer
    /// </summary>
    public class long3 : Vector3<long> { }

    /// <summary>
    /// A 3-Component unsigned 64 bit integer
    /// </summary>
    public class ulong3 : Vector3<ulong> { }

    /// <summary>
    /// A 3-Component unsigned 32 bit floating point
    /// </summary>
    public class float3 : Vector3<float> { }

    /// <summary>
    /// A 3-Component unsigned 64 bit floating point 
    /// </summary>
    public class double3 : Vector3<double> { }
}
