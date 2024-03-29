﻿using System;
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

            head = head.Replace("[NonKernel]", "");
            head = head.Replace("[NonKernelAttribute]", "");

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
        /// Translate body of kernel
        /// </summary>
        private string TranslateBody(string body)
        {
            // Remove extra indentations
            string bodyContent = MethodParseHelpers.ExtractBodyContent(body);

            bodyContent = bodyContent.Replace("args.get_global_id", "get_global_id");
            bodyContent = bodyContent.Replace("args.get_local_id", "get_local_id");
            bodyContent = bodyContent.Replace("(byte)", "(unsigned char)");

            return "{\n" + bodyContent + "\n}";
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
