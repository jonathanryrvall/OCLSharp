using System;
using System.Collections.Generic;
using System.Text;

namespace OCLSharp.Translating
{
    /// <summary>
    /// Find classes in a code file
    /// </summary>
    public class ClassFinder
    {
        private string code;

        public ClassFinder(string code)
        {
            this.code = code;
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
        /// Returns all classes in a code
        /// </summary>
        private IEnumerable<string> FindClasses(string code)
        {
            for (int i = 0; i < code.Length - 5; i++)
            {
                if (code.Substring(i, 5) == "class")
                {
                    //// Commented
                    //if (IsRowComment(i, code))
                    //{
                    //    continue;
                    //}

                    string classCode = FindClass(code, i);

                    // Class found!
                    if (!string.IsNullOrEmpty(classCode))
                    {
                        yield return classCode;
                    }
                }
            }
        }

        public string GetCombinedCode()
        {
            string combined = "";

            foreach(string c in FindClasses(code))
            {
                combined += c + "\n\n\n\n";
            }

            return combined;
        }
    }
}
