using System;
using System.Collections.Generic;
using System.Text;

namespace OCLSharp.Attributes
{
    /// <summary>
    /// Marks a memory pointer as belonging to the __global address space
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class GlobalAttribute : Attribute
    {

    }

}
