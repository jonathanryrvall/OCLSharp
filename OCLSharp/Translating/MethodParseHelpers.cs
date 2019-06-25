using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace OCLSharp.Translating
{
    public class MethodParseHelpers
    {


        /// <summary>
        /// Separate method head from body
        /// </summary>
        public static string[] Decaptitate(string code)
        {
            int paranthesesBalance = 0;
            int lastParantheses = -1;

            // Iterate through code until parantheses found
            for (int i = 0; i < code.Length; i++)
            {
                // Find starting parantheses
                if (code[i] == '(')
                {
                    paranthesesBalance++;
                }

                // Find end parantheses
                if (code[i] == ')')
                {
                    paranthesesBalance--;

                    if (paranthesesBalance == 0)
                    {
                        lastParantheses = i;
                        break;
                    }
                }
            }

            string[] split = new string[2];
            split[0] = code.Substring(0, lastParantheses + 1);
            split[1] = code.Substring(lastParantheses + 1, code.Length - 1 - lastParantheses);

            return split;
        }

        /// <summary>
        /// Removes tabs, newlines and extra spaces from head of method
        /// </summary>
        public static string RemoveHeadSpaces(string head)
        {
            // Remove tabs, newlines
            head = head.Replace("\t", " ");
            head = head.Replace("\n", "");

            // Remove extra spaces
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[ ]{2,}", options);
            head = regex.Replace(head, " ");

            return head;
        }

        /// <summary>
        /// Extract content withing method body
        /// </summary>
        public static string ExtractBodyContent(string body)
        {
            // Find start and end bracket
            int startBracket = body.IndexOf('{');
            int endBracket = body.LastIndexOf('}');
            
            // Body content extracted
            string extract = body.Substring(startBracket + 1, endBracket - 1 - startBracket);

            // Find first and last line that is actual code
            string[] lines = extract.Split(new string[] { "\n" }, StringSplitOptions.None);
            int firstLinesIndex = -1;
            int lastLinesIndex = -1;

            // Find first line
            for (int i= 0; i < lines.Length; i++)
            {
                if (Regex.Matches(lines[i], @"[a-zA-Z0-9]").Count > 0)
                {
                    firstLinesIndex = i;
                    break;
                }
            }

            // Find last line
            for (int i = lines.Length - 1; i >= 0; i--)
            {
                if (Regex.Matches(lines[i], @"[a-zA-Z0-9]").Count > 0)
                {
                    lastLinesIndex = i;
                    break;
                }
            }

            // Count leading spaces on first actual line
            int leadingSpaces = 0;
            for (int s = 0; s < lines[firstLinesIndex].Length; s++)
            {
                if (lines[firstLinesIndex][s] == ' ')
                {
                    leadingSpaces++;
                }
                else
                {
                    break;
                }
            }
            

            // Create replacement pattern for all
            if (leadingSpaces > 4)
            {
                int excessSpaces = leadingSpaces - 4;

                for (int i = 0; i < lines.Length; i++)
                {
                    // Line is too short!
                    if (lines[i].Length < excessSpaces)
                    {
                        continue;
                    }

                    // If there is any character within the excess space range that is not a space
                    if (lines[i].Substring(0, excessSpaces).Any(c => c != ' '))
                    {
                        continue;
                    }
                       
                    // Remove excess spaces from line
                    lines[i] = lines[i].Substring(excessSpaces, lines[i].Length - excessSpaces);
                }
            }

            // Combine lines
            string combined = "";
            for (int i = firstLinesIndex; i < lastLinesIndex + 1; i++)
            {
                combined += lines[i];

                // Add newlines unless it is the last line
                if (i < lastLinesIndex)
                {
                    combined += "\n";
                }
            }
            

            return combined;
        }

        ///// <summary>
        ///// Removes extra indentations from method body
        ///// </summary>
        //public static string RemoveExtraIndentationsFromBody(string body)
        //{
        //    // Replace tabs with 4 spaces
        //    body = body.Replace("\t", "    ");

        //    // Find 
        //}


    }
}
