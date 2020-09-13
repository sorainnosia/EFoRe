using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Efore
{
    public class FileFolderKey
    {
        public List<string> Keywords { get; set; }
        public string FolderShortName { get; set; }
        public string FolderName { get; set; }

        public static FileFolderKey GetKey(string folderpath)
        {
            List<string> keywords = new List<string>();

            string directoryname = Path.GetFileNameWithoutExtension(folderpath);
            directoryname = directoryname.Replace('_', ' ');
            directoryname = RemoveBracket(directoryname);
            string[] words = directoryname.Split(new char[] { ' ' });
            foreach (string word in words)
            {
                string w = FolderParser.GetNormalWord(word);
                if (string.IsNullOrEmpty(w) == false && w != "-" && w.ToUpper() != "COPY")
                {
                    keywords.Add(w);
                }
            }
            return new FileFolderKey { Keywords = keywords, FolderShortName = Path.GetFileName(folderpath), FolderName = folderpath };
        }

        public static string GetNewFolder(string folderpath)
        {
            List<string> keywords = new List<string>();

            string directoryname = Path.GetFileNameWithoutExtension(folderpath);
            directoryname = directoryname.Replace('_', ' ');
            directoryname = RemoveBracket(directoryname);
            string[] words = directoryname.Split(new char[] { ' ' });
            foreach (string word in words)
            {
                string w = FolderParser.GetNormalWord(word, false);
                if (string.IsNullOrEmpty(w) == false && w != "-" && w.ToUpper() != "COPY")
                {
                    keywords.Add(w);
                }
            }
            return Path.Combine(Path.GetDirectoryName(folderpath), StringJoin(" ", keywords));
        }

        public static string StringJoin(string t, List<string> keyword)
        {
            string result = string.Empty;
            foreach (string str in keyword)
            {
                result += str + t;
            }
            if (result.EndsWith(t))
            {
                result = result.Substring(0, result.Length - t.Length);
            }
            return result;
        }

        public static string RemoveBracket(string str)
        {
            string result = string.Empty;
            bool ignore = false;
            foreach (char c in str)
            {
                if (c == '[' || c == '(')
                {
                    ignore = true;
                }
                else
                {
                    if (ignore == false && c != ']' && c != ')')
                    {
                        result += c;
                    }
                    else
                    {
                        if (c == ']' || c == ')')
                        {
                            ignore = false;
                        }
                    }
                }
            }
            return result.Trim();
        }
    }
}
