using System;
using System.Collections.Generic;
using System.Text;

namespace OCLSharp.Attributes
{
   
    /// <summary>
    /// Marks a memory pointer as read and writable
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class ReadWriteAttribute : Attribute
    {

    }

}
