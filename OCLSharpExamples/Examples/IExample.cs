using System;
using System.Collections.Generic;
using System.Text;

namespace OCLSharpExamples.Examples
{
    interface IExample
    {
        void Run();
        string Description { get; }
    }
}
