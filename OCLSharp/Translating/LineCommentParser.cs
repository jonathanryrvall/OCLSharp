using System;
using System.Collections.Generic;
using System.Text;

namespace OCLSharp.Translating
{
    public class LineCommentParser
    {
        /// <summary>
        /// Check if code at specific index is start of a line comment
        /// </summary>
        public static bool GetLineCommentStart(string code, int index)
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
        public static int GetLineCommentEnd(string code, int index, out string lineComment)
        {
            // Search for newline
            for (int i = index; i < code.Length; i++)
            {
                // Check if character is newline
                if (code[i] == '\n')
                {
                    lineComment = code.Substring(index, i + 1 - index);
                    return i;
                }
            }

            // No end found
            lineComment = code.Substring(index, code.Length - index);

            return code.Length;
        }
    }
}
