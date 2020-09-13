using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Efore
{
    public class FolderManager
    {
        public List<string> ScanEmptyFolder(string folderPath)
        {
            List<string> emptyFolderResult = new List<string>();
            AddEmptyFolder(0, emptyFolderResult, folderPath);
            return emptyFolderResult;
        }

        public int DeleteEmptyFolder(List<string> folders)
        {
            int deleted = 0;
            foreach (string folder in folders)
            {
                if (Directory.Exists(folder))
                {
                    try
                    {
                        Directory.Delete(folder);
                        deleted++;
                    }
                    catch
                    {
                    }
                }
            }
            return deleted;
        }

        protected void AddEmptyFolder(int depth, List<string> emptyFolderResult, string folderPath)
        {
            int count = 0;
            if (Directory.Exists(folderPath) == false) return;

            string[] files = Directory.GetFiles(folderPath);
            if (files == null || files.Length == 0)
            {
                if (depth != 0) emptyFolderResult.Add(folderPath);
            }
            string[] directories = Directory.GetDirectories(folderPath);
            if (directories == null || directories.Length == 0) return;
            foreach (string directory in directories)
            {
                AddEmptyFolder(depth + 1, emptyFolderResult, directory);
            }
        }

        public int Rapihin (string folderPath)
        {
            if (Directory.Exists(folderPath) == false) throw new Exception("Directory does not exist");

            Dictionary<string, FileFolderKey> folders = IndexFolder(folderPath);
            Dictionary<string, FileFolderKey> files = IndexFile(folderPath);

            return DoRapihin(folders, files);
        }

        protected Dictionary<string, FileFolderKey> IndexFolder(string path)
        {
            Dictionary<string, FileFolderKey> result = new Dictionary<string, FileFolderKey>();
            string[] directories = Directory.GetDirectories(path);
            foreach (string directory in directories)
            {
                FileFolderKey key = FileFolderKey.GetKey(directory);
                result.Add(Path.GetFileName(directory), key);
            }
            return result;
        }

        protected Dictionary<string, FileFolderKey> IndexFile(string path)
        {
            Dictionary<string, FileFolderKey> result = new Dictionary<string, FileFolderKey>();
            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                if (IsVideoFile(file))
                {
                    FileFolderKey key = FileFolderKey.GetKey(file);
                    if (IsIgnore(key) == false)
                    {
                        result.Add(Path.GetFileName(file), key);
                    }
                }
            }
            return result;
        }

        protected bool IsVideoFile(string path)
        {
            string extension = Path.GetExtension(path).ToUpper();
            switch (extension)
            {
                case ".AAF":
                case ".3GP":
                case ".ASF":
                case ".AVI":
                case ".FLV":
                case ".FLA":
                case ".MKV":
                case ".M4V":
                case ".MPEG":
                case ".MPG":
                case ".MPE":
                case ".OGG":
                case ".RM":
                case ".WMV":
                case ".MP4":
                    return true;
            }
            return false;
        }

        protected bool IsIgnore(FileFolderKey file)
        {
            if (Path.GetExtension(file.FolderName).ToUpper() == ".EXE") return true;
            if (Path.GetExtension(file.FolderName).ToUpper() == ".PDB") return true;
            if (Path.GetExtension(file.FolderName).ToUpper() == ".MANIFEST") return true;
            if (Path.GetExtension(file.FolderName).ToUpper() == ".CONFIG") return true;
            return false;
        }

        protected int DoRapihin(Dictionary<string, FileFolderKey> folders, Dictionary<string, FileFolderKey> files)
        {
            int totalMoves = 0;
            foreach (KeyValuePair<string, FileFolderKey> file in files)
            {
                double totalWord = (double)file.Value.Keywords.Count;
                double matchWord = (double)0;
                bool isMove = false;

                foreach (KeyValuePair<string, FileFolderKey> folder in folders)
                {
                    int totalFolderWord = folder.Value.Keywords.Count;
                    double iteration = totalWord > totalFolderWord ? totalWord : totalFolderWord;
                    int startIndex = 0;
                    for (int i = 0; i < iteration; i++)
                    {
                        if (i >= file.Value.Keywords.Count) break;
                        if (i >= folder.Value.Keywords.Count) break;
                        if (folder.Value.Keywords[i] == file.Value.Keywords[startIndex])
                        {
                            matchWord++;
                            startIndex++;
                        }
                    }
                    double percent = (matchWord / totalWord) * 100;
                    if (percent > 60)
                    {
                        try
                        {
                            DoMove(file.Value, folder.Value);
                            totalMoves++;
                            isMove = true;
                            break;
                        }
                        catch { }
                    }
                }

                if (isMove == false)
                {
                    try
                    {
                        string newFilePath = GetNewFolderName(file.Value);
                        CreateFolder(newFilePath);
                        DoMove(file.Value, newFilePath);
                        totalMoves++;
                    }
                    catch { }
                }
            }
            return totalMoves;
        }

        protected string GetNewFolderName(FileFolderKey file)
        {
            return FileFolderKey.GetNewFolder(file.FolderName);
        }

        protected void CreateFolder(string path)
        {
            if (Directory.Exists(path) == false)
                Directory.CreateDirectory(path);
        }

        protected void DoMove(FileFolderKey file, FileFolderKey folder)
        {
            File.Move(file.FolderName, Path.Combine(folder.FolderName, file.FolderShortName));
        }

        protected void DoMove(FileFolderKey file, string folder)
        {
            File.Move(file.FolderName, Path.Combine(folder, file.FolderShortName));
        }
    }
}
