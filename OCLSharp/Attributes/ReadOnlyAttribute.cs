using System;
using System.Collections.Generic;
using System.Text;

namespace OCLSharp.Attributes
{
    /// <summary>
    /// Marks a memory pointer as read only
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class ReadOnlyAttribute : Attribute
    {

    }
  
}
