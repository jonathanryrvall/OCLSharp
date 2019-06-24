using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace OCLSharp
{


    /// <summary>
    /// Translator that translated C# to OpenCL
    /// </summary>
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
            string openCLCode = "";

            //// Split code based on line endings
            //var lines = GetLines(csCode);

            //// Iterate through each line
            //foreach (string line in lines)
            //{
            //    string result = ProcessLine(line);

            //    // If result is not empty add it to code
            //    if (!string.IsNullOrEmpty(result))
            //    {
            //        openCLCode += result + "\r\n";
            //    }
            //}

            ////  string[] lines = GetLines(csCode);
            //bool isLineComment = false;
            //bool isMultiLineComment = false;

            //// Replace carriage returns with newlines
            //string tempCode = csCode.Replace("\r\n", "\n");

            //for (int i = 0; i < tempCode.Length; i++)
            //{
            //    // Check for line comment
            //    if (tempCode.Substring(i,2) == "//" &&
            //        !(isMultiLineComment || isLineComment))
            //    {
            //        isLineComment = true;
            //    }

            //    // Check for line comment end
            //    if (tempCode.Substring(i, 2) == "\n" &&
            //        isLineComment)
            //    {
            //        isLineComment = false;
            //    }

            //    // Check for multiline comment
            //    if (tempCode.Substring(i, 2) == "/*" &&
            //        !(isMultiLineComment || isLineComment))
            //    {
            //        isMultiLineComment = true;
            //    }

            //    // Check for multiline comment end
            //    if (tempCode.Substring(i, 2) == "*/" &&
            //        isMultiLineComment)
            //    {
            //        isMultiLineComment = true;
            //    }
            //}




            // Remove using directives at top of code
            //openCLCode = RemoveUsings(openCLCode);

            foreach(string classCode in FindClasses(csCode))
            {
                foreach (string nonKernelCode in FindNonKernels(classCode))
                {
                    openCLCode += CleanMethod(nonKernelCode) + "\r\n";
                }
                foreach (string kernelCode in FindKernels(classCode))
                {
                    openCLCode += CleanMethod(kernelCode) + "\r\n";
                }
                
            }
           
            return openCLCode;
        }

        private string CleanMethod(string code)
        {
            code = code.Replace("[NonKernel]", "");
            code = code.Replace("[Kernel]", "__kernel");

            code = code.Replace("public", "");
            code = code.Replace("private", "");

            code = code.Replace("[Global]", "__global");

            code = code.Replace("byte[]", "unsigned char*");
            code = code.Replace("short[]", "short*");
            code = code.Replace("int[]", "int*");
            code = code.Replace("long[]", "long*");

            // Remove WorkItemArgs
            code = code.Replace("WorkItemArgs args,", "");
            code = code.Replace("args.GetGlobalID", "get_global_id");
            code = code.Replace("args.GetLocalID", "get_local_id");


            return code;
        }

   

      

        /// <summary>
        /// Returns all classes in a code
        /// </summary>
        private IEnumerable<string> FindClasses(string code)
        {
            for (int i = 0; i < code.Length - 5; i++)
            {
                if (code.Substring(i, 5) == "class")
                {
                    string classCode = FindClass(code, i);

                    // Class found!
                    if (!string.IsNullOrEmpty(classCode))
                    {
                        yield return classCode;
                    }
                }
            }
        }

        /// <summary>
        /// Returns all classes in a code
        /// </summary>
        private IEnumerable<string> FindNonKernels(string code)
        {
            for (int i = 0; i < code.Length - 11; i++)
            {
                if (code.Substring(i, 11) == "[NonKernel]")
                {
                    string kernelCode = FindKernel(code, i);

                    // Class found!
                    if (!string.IsNullOrEmpty(kernelCode))
                    {
                        yield return kernelCode;
                    }
                }
            }
        }

        /// <summary>
        /// Returns all classes in a code
        /// </summary>
        private IEnumerable<string> FindKernels(string code)
        {
            for (int i = 0; i < code.Length - 8; i++)
            {
                if (code.Substring(i, 8) == "[Kernel]")
                {
                    string kernelCode = FindKernel(code, i);

                    // Class found!
                    if (!string.IsNullOrEmpty(kernelCode))
                    {
                        yield return kernelCode;
                    }
                }
            }
        }

        /// <summary>
        /// Find class!
        /// </summary>
        private string FindKernel(string code, int classStart)
        {
            int bracketBalance = 0;
            int lastBracket = -1;

            // Iterate through code until brackets found
            for (int i = classStart; i < code.Length; i++)
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

            return code.Substring(classStart, lastBracket + 1 - classStart);
        }

        /// <summary>
        /// Find class!
        /// </summary>
        private string FindClass(string code, int classStart)
        {
            int bracketBalance = 0;
            int firstBracket = -1;
            int lastBracket = -1;

            // Iterate through code until brackets found
            for (int i = classStart; i < code.Length; i++)
            {
                // Find starting bracket
                if (code[i] == '{')
                {
                    if (firstBracket == -1)
                    {
                        firstBracket = i + 1;
                    }
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

            return code.Substring(firstBracket, lastBracket - firstBracket);
        }

       
        /// <summary>
        /// Split code into lines
        /// </summary>
        private string[] GetLines(string code)
        {
            return code.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
        }

        /// <summary>
        /// Split code into array based on comment start and end
        /// </summary>
        private string[] CommentSplit(string code)
        {
            var separators = new string[]
            {
                "\r\n",
                "\n",
                "//",
                "/*",
                "*/"
            };
            return code.Split(separators, StringSplitOptions.None);
        }

    }
}
