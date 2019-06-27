using System;
using System.Collections.Generic;
using System.Text;

namespace OCLSharp.OpenCL.DataTypes.VectorDataTypes
{
    //public struct int2
    //{
    //    public int x, y;


    //    public static implicit operator int2((uint, uint) d)
    //    {
    //        return new int2();
    //    }
    //}
    //public struct int3
    //{
    //    public int x, y;


    //    public static implicit operator int2((uint, uint) d)
    //    {
    //        return new int2();
    //    }
    //}
    //public class int2
    //{
    //    public int x, y;


    //}

    public class int2 : Vector2<int> { }
    public class float2 : Vector2<int> { }

    public class Vector2<T>
    {
        public dynamic x, y;

        public static Vector2<T> operator +(Vector2<T> a, Vector2<T> b)
        {
            return new Vector2<T>()
            {
                x = a.x + b.x,
                y = a.y + b.y
            };
        }

    }

    //public class Vector2<T>
    //{
    //    public T x, y;

    //    public static Vector2<T> operator +(Vector2<T> a, Vector2<T> b)
    //    {
    //        return new Vector2<T>()
    //        {
    //            x = (dynamic)a.x + (dynamic)b.x,
    //            y = (dynamic)a.y + (dynamic)b.y
    //        };
    //    }

    //}


}
