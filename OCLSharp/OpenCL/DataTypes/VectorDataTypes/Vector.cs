using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace OCLSharp.OpenCL.DataTypes.VectorDataTypes
{
    /// <summary>
    /// Generic vector base class
    /// </summary>
    /// <typeparam name="T">Datatype contained in vector</typeparam>
    public abstract class Vector<T>
    {
    
        /// <summary>
        /// + Operator
        /// </summary>
        public static dynamic operator +(Vector<T> a, Vector<T> b)
        {
            return PerformOperator((va, vb) => va + vb, a, b);
        }

        /// <summary>
        /// - Operator
        /// </summary>
        public static dynamic operator -(Vector<T> a, Vector<T> b)
        {
            return PerformOperator((va, vb) => va - vb, a, b);
        }


        private static Vector<T> PerformOperator(Func<dynamic,dynamic, dynamic> func,
                                            Vector<T> a,
                                            Vector<T> b)
        {
            Type type = a.GetType();
            Vector<T> result = (Vector<T>)Activator.CreateInstance(type);

            FieldInfo[] fields = type.GetFields();
            foreach (FieldInfo field in fields)
            {
                dynamic aValue = field.GetValue(a);
                dynamic bValue = field.GetValue(b);
                dynamic cValue = func(aValue, bValue);
                field.SetValue(result, cValue);
            }
            return result;
        }


    }
}
