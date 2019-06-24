using System;
using System.Collections.Generic;
using System.Text;

namespace OCLSharp.Translating
{
    public class NonKernelParser
    {
        private string code;

        public NonKernelParser(string code)
        {
            this.code = code;
        }

        /// <summary>
        /// Find end of a non kernel
        /// </summary>
        public static int GetNonKernelEnd(string code, int index, out string nonKernel)
        {
            int bracketBalance = 0;
            int lastBracket = -1;

            // Iterate through code until brackets found
            for (int i = index; i < code.Length; i++)
            {
                // Find starting bracket
                if (code[i] == '{')
                {
                    bracketBalance++;
                }

                // Find end bracket
                if (code[i] == '}')
                {
                    bracketBalance--;
                    if (bracketBalance == 0)
                    {
                        lastBracket = i;
                        break;
                    }
                }
            }

            nonKernel = code.Substring(index, lastBracket + 1 - index);

            return lastBracket;
        }


        /// <summary>
        /// Check if code at specific index is start of a non - kernel method
        /// </summary>
        public static bool GetNonKernelStart(string code, int index)
        {
            // Out of range
            if (index > code.Length - 20)
            {
                return false;
            }


            // Start of kernel
            if (code.Substring(index, 11) == "[NonKernel]")
            {
                return true;
            }
            if (code.Substring(index, 20) == "[NonKernelAttribute]")
            {
                return true;
            }

            // Not the start of a non - kernel
            return false;
        }
    }
}
