using OCLSharp.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace OCLSharp.Translating
{
    public class Translator
    {
        private string csCode;

        /// <summary>
        /// Create an empty <see cref="Translator"/>
        /// </summary>
        public Translator() { }

        /// <summary>
        /// Create a new <see cref="Translator"/> from C# source code
        /// </summary>
        /// <param name="csCode">Code in C# language</param>
        public Translator(string csCode)
        {
            this.csCode = csCode;
        }

        /// <summary>
        /// Translates C# code to OpenCL
        /// </summary>
        /// <returns>OpenCL code</returns>
        public string Translate()
        {
            string result = "";

            // Step 1
            // Extract all code within all classes in the file
            string classExtractionResult = new ClassFinder(csCode).GetCombinedCode();

            // Step 2 iterate through all code extracted
            for (int i = 0; i < csCode.Length; i++)
            {
                // Start of line comment
                if (GetLineCommentStart(csCode, i))
                {
                    // Get end of comment
                    i = GetLineCommentEnd(csCode, i, out string lineComment);

                    // Appent comment to result
                    result += lineComment;
                }

                // Start of non kernel method
                if (NonKernelParser.GetNonKernelStart(csCode, i))
                {
                    // Get end of comment
                    i = NonKernelParser.GetNonKernelEnd(csCode, i, out string nonKernel);

                    // Appent comment to result
                    result += nonKernel + "\n\n";
                }


            }




            return result;
        }

      

        /// <summary>
        /// Check if code at specific index is start of a line comment
        /// </summary>
        private bool GetLineCommentStart(string code, int index)
        {
            // Out of range
            if (index > code.Length - 2)
            {
                return false;
            }

            // Check if start of line comment
            return code.Substring(index, 2) == "//";
        }

        /// <summary>
        /// Check if code at specific index is start of a line comment
        /// </summary>
        private int GetLineCommentEnd(string code, int index, out string lineComment)
        {
            // Search for newline
            for (int i = index; i < code.Length; i++)
            {
                // Check if character is newline
                if (code[i] == '\n')
                {
                    lineComment = code.Substring(index, i - index);
                    return i;
                }
            }

            // No end found
            lineComment = code.Substring(index, code.Length - index);

            return code.Length;
        }




    }
}
