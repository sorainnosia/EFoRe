using System;
using System.Collections.Generic;
using System.Text;

namespace Efore
{
    public class FolderParser
    {
        public static string GetNormalWord(string word, bool toupper = true)
        {
            if (word.StartsWith("[") && word.EndsWith("]")) return string.Empty;
            if (word.StartsWith("(") && word.EndsWith(")")) return string.Empty;
            word = word.Replace("!", "");
            word = word.Replace("@", "");
            word = word.Replace("#", "");
            word = word.Replace("$", "");
            word = word.Replace("%", "");
            word = word.Replace("^", "");
            word = word.Replace("&", "");
            word = word.Replace("*", "");
            word = word.Replace("(", "");
            word = word.Replace(")", "");
            word = word.Replace("[", "");
            word = word.Replace("]", "");
            word = word.Replace("{", "");
            word = word.Replace("}", "");
            word = word.Replace(":", "");
            word = word.Replace(";", "");
            word = word.Replace("\"", "");
            word = word.Replace("'", "");
            word = word.Replace("|", "");
            word = word.Replace("\\", "");
            word = word.Replace("<", "");
            word = word.Replace(">", "");
            word = word.Replace(",", "");
            word = word.Replace(".", "");
            word = word.Replace("?", "");
            word = word.Replace("/", "");
            word = word.Replace("-", "");
            word = word.Replace("_", "");
            word = word.Replace("+", "");
            word = word.Replace("=", "");
            word = word.Replace("`", "");
            if (toupper)
            {
                word = word.ToUpper();
            }
            foreach (char c in word)
            {
                if (c > '0' && c <= '9') return string.Empty;
            }
            if (word.ToUpper().Trim().StartsWith("COPY") == true && word.Trim().Length == 7 ) return string.Empty;
            return word;
        }
    }
}
