using System;
using System.Collections.Generic;
using System.Text;

namespace OCLSharp.Translating
{
    public class ConstantFieldParser
    {
        private string code;

        public ConstantFieldParser(string code)
        {
            this.code = code;
        }

        /// <summary>
        /// Find end of a constant field
        /// </summary>
        public static int GetConstantFieldEnd(string code, int index, out string constantField)
        {
            // Iterate through code until semicolon found
            for (int i = index; i < code.Length; i++)
            {
                // Find end semicolon
                if (code[i] == ';')
                {
                    constantField = code.Substring(index, i + 1 - index);
                    return i;
                }
            }

            // Nothing found!
            constantField = "";
            return code.Length;
        }


        /// <summary>
        /// Check if code at specific index is start of a non - kernel method
        /// </summary>
        public static bool GetConstantFieldStart(string code, int index)
        {
            // Out of range
            if (index > code.Length - 19)
            {
                return false;
            }

            // Start of kernel
            if (code.Substring(index, 10) == "[Constant]")
            {
                return true;
            }
            if (code.Substring(index, 19) == "[ConstantAttribute]")
            {
                return true;
            }

            // Not the start of a non - kernel
            return false;
        }

      
        private class ConstantFieldParts
        {
            public string Name;
            public string DataType;
            public string Data;
        }



        /// <summary>
        /// Translate C# cernel to OpenCL kernel
        /// </summary>
        public string Translate()
        {
            // Access modifiers
            code = code.Replace("[Constant]", "__constant");

            // Constant attribute
            code = code.Replace("[Constant]", "__constant");
            code = code.Replace("[ConstantAttribute]", "__constant");

            // Remove access modifiers
            code = code.Replace(" public ", " ");
            code = code.Replace(" private ", " ");

            // Array
            if (code.Contains("[]"))
            {
                code = code.Replace("[]", "");
                code = code.Replace("=", "[] =");
            }

            return code;

        }
    }
}
