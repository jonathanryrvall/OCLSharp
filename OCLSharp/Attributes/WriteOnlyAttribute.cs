using System;
using System.Collections.Generic;
using System.Text;

namespace OCLSharp.Attributes
{
    /// <summary>
    /// Marks a memory pointer as write only
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class WriteOnlyAttribute : Attribute
    {

    }
    
}
