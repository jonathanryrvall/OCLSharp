using System;
using System.Collections.Generic;
using System.Text;

namespace OCLSharp.Attributes
{
    /// <summary>
    /// Marks a method as an OpenCL Kernel
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class NonKernelAttribute : Attribute
    {

    }
}
