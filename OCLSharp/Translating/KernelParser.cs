using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace OCLSharp.Translating
{


    public class KernelParser
    {
        private string code;

        public KernelParser(string code)
        {
            this.code = code;
        }

        /// <summary>
        /// Find end of a kernel
        /// </summary>
        public static int GetKernelEnd(string code, int index, out string kernel)
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

            kernel = code.Substring(index, lastBracket + 1 - index);

            return lastBracket;
        }


        /// <summary>
        /// Check if code at specific index is start of a non - kernel method
        /// </summary>
        public static bool GetKernelStart(string code, int index)
        {
            // Out of range
            if (index > code.Length - 17)
            {
                return false;
            }


            // Start of kernel
            if (code.Substring(index, 8) == "[Kernel]")
            {
                return true;
            }
            if (code.Substring(index, 17) == "[KernelAttribute]")
            {
                return true;
            }

            // Not the start of a non - kernel
            return false;
        }


        /// <summary>
        /// Translate head part of kernel
        /// </summary>
        private string TranslateHead(string head)
        {
            // Remove tabs, newlines and extra spaces
            head = MethodParseHelpers.RemoveHeadSpaces(head);

            // Remove WorkItemArgs
            head = head.Replace("WorkItemArgs args,", "");

            // Replace attributes
            head = head.Replace("[Global]", "__global");
            head = head.Replace("[GlobalAttribute]", "__global");

            head = head.Replace("[Kernel]", "__kernel");
            head = head.Replace("[KernelAttribute]", "__kernel");

            head = head.Replace("[ReadOnly]", "__read_only");
            head = head.Replace("[ReadOnlyAttribute]", "__read_only");

            head = head.Replace("[WriteOnly]", "__write_only");
            head = head.Replace("[WriteOnlyAttribute]", "__write_only");

            head = head.Replace("[ReadWrite]", "__write_only");
            head = head.Replace("[ReadWriteAttribute]", "__read_write");




            // Remove access modifiers
            head = head.Replace(" public ", " ");
            head = head.Replace(" private ", " ");


            // Add newline between arguments
            head = head.Replace(",", ",\n");

            // Replace array pointers
            head = head.Replace("[]", "*");

            // Replace c# datatypes
            head = head.Replace("byte", "unsigned char");

            return head;
        }

        /// <summary>
        /// Count how many "blank characters" there are at the beginning of a string
        /// </summary>
        private int CountLeadingBlankCharacters(string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] != ' ' &&
                    str[i] != '\n')
                {
                    return i;
                }
            }
            return 0;
        }

        /// <summary>
        /// Translate local memory declaration within kernel body
        /// </summary>
        private string TranslateLocalMemoryWithinBody(string body)
        {
            while (body.Contains("GetLocalMem"))
            {
                int localMemIndex = body.IndexOf("GetLocalMem");
                int startIndex = FindLocalMemStart(localMemIndex, body);
                int endIndex = FindLocalMemEnd(localMemIndex, body);

                string localMemString = body.Substring(startIndex, endIndex + 1 - startIndex);

                int leadingBlanks = CountLeadingBlankCharacters(localMemString);
                startIndex += leadingBlanks;
                localMemString = body.Substring(startIndex, endIndex + 1 - startIndex);

                string newLocalMemString = TranslateLocalMemoryDeclaration(localMemString);
                // Remove old declaration
                body = body.Remove(startIndex, endIndex + 1 - startIndex);

                // Insert new translated declaration
                body = body.Insert(startIndex, newLocalMemString);

            }

            return body;
        }

        /// <summary>
        /// Translate declaration of local memory
        /// </summary>
        private string TranslateLocalMemoryDeclaration(string declaration)
        {
            declaration = declaration.Replace(" ", "");

            int getMemIndex = declaration.IndexOf("GetLocalMem(args,new");

            int fullDataTypeStartIndex = getMemIndex + 20;
            int fullDataTypeEndIndex = declaration.LastIndexOf(",");
            int nameStartIndex = declaration.IndexOf("]") + 1;

            string name = declaration.Substring(nameStartIndex, getMemIndex - 1 - nameStartIndex);
            string fullDataType = declaration.Substring(fullDataTypeStartIndex, fullDataTypeEndIndex - fullDataTypeStartIndex);
            string dataType = fullDataType.Substring(0, fullDataType.IndexOf("["));

            int dataLengthStart = fullDataType.IndexOf("[");
            int dataLengthEnd = fullDataType.IndexOf("]");

            string dataLength = fullDataType.Substring(dataLengthStart + 1, dataLengthEnd - 1 - dataLengthStart);

            return $"__local {dataType} {name}[{dataLength}];";
        }

        /// <summary>
        /// Find start of local memory declaration within kernel body
        /// </summary>
        private int FindLocalMemEnd(int refPoint, string body)
        {
            for (int i = refPoint; i < code.Length; i++)
            {
                if (body[i] == ';')
                {
                    return i;
                }

            }

            return 0;
        }

        /// <summary>
        /// Find end of local memory declaration within kernel body
        /// </summary>
        private int FindLocalMemStart(int refPoint, string body)
        {
            for (int i = refPoint; i >= 0; i--)
            {
                if (body[i] == ';' ||
                    body[i] == '{' ||
                    body[i] == '}')
                {
                    return i + 1;
                }

            }

            return 0;
        }

        /// <summary>
        /// Translate body of kernel
        /// </summary>
        private string TranslateBody(string body)
        {
            // Remove extra indentations
            body = MethodParseHelpers.ExtractBodyContent(body);

            body = body.Replace("args.get_global_id", "get_global_id");
            body = body.Replace("args.get_global_size", "get_global_size");

            body = body.Replace("args.get_local_id", "get_local_id");
            body = body.Replace("args.get_local_size", "get_local_size");
            body = body.Replace("(byte)", "(unsigned char)");

            // Local memory declared within body
            body = TranslateLocalMemoryWithinBody(body);

            // Barriers
            body = body.Replace("barrier(args,", "barrier(");


            return "{\n" + body + "\n}";
        }

        /// <summary>
        /// Translate C# cernel to OpenCL kernel
        /// </summary>
        public string Translate()
        {
            string[] split = MethodParseHelpers.Decaptitate(code);
            string result = "";

            // Translate head of kernel method
            result += TranslateHead(split[0]);

            // Newline between head and body
            result += "\n";

            // Translate body of kernel method
            result += TranslateBody(split[1]);

            return result;

        }
    }
}
