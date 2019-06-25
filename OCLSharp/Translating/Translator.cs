using OCLSharp.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace OCLSharp.Translating
{
    public class Translator
    {
        private string csCode;
        private bool includeCommentsOutsideMethods;

        /// <summary>
        /// Create an empty <see cref="Translator"/>
        /// </summary>
        public Translator() { }

        /// <summary>
        /// Create a new <see cref="Translator"/> from C# source code
        /// </summary>
        /// <param name="csCode">Code in C# language</param>
        /// <param name="includeCommentsOutsideMethods">Code that has been commented out outside of any method should be included</param>
        public Translator(string csCode,
                          bool includeCommentsOutsideMethods = false)
        {
         
            // Use common newline
            this.csCode = csCode.Replace("\r\n", "\n");

            this.includeCommentsOutsideMethods = includeCommentsOutsideMethods;
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
            csCode = classExtractionResult;


            // Step 2 iterate through all code extracted
            for (int i = 0; i < csCode.Length; i++)
            {
                // Start of line comment
                if (LineCommentParser.GetLineCommentStart(csCode, i))
                {
                    // Get end of comment
                    i = LineCommentParser.GetLineCommentEnd(csCode, i, out string lineComment);

                    // Append to result
                    if (includeCommentsOutsideMethods)
                    {
                        result += lineComment;
                    }
                }

                // Start of non kernel method
                if (NonKernelParser.GetNonKernelStart(csCode, i))
                {
                    // Get end of non kernel
                    i = NonKernelParser.GetNonKernelEnd(csCode, i, out string nonKernel);

                    // Append to result
                    result += new NonKernelParser(nonKernel).Translate() + "\n\n";
                }

                // Start of kernel method
                if (KernelParser.GetKernelStart(csCode, i))
                {
                    // Get end of kernel
                    i = KernelParser.GetKernelEnd(csCode, i, out string kernel);

                    // Append to result
                    result += new KernelParser(kernel).Translate() + "\n\n";
                }

                // Start of constant field
                if (ConstantFieldParser.GetConstantFieldStart(csCode, i))
                {
                    // Get end of constant field
                    i = ConstantFieldParser.GetConstantFieldEnd(csCode, i, out string constantField);

                    // Append to result
                    result += new ConstantFieldParser(constantField).Translate() + "\n\n";
                }

            }




            return result;
        }

    }
}
