using System;
using System.Collections.Generic;
using System.Text;

namespace OCLSharp.Attributes
{
    /// <summary>
    /// Marks a memory pointer as belonging to the __local address space
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class LocalAttribute : Attribute
    {

    }
}
