using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Detached.Mvc.Localization
{
    public class FileSystem : IFileSystem
    {
        public string[] GetFiles(string dirPath, string pattern = "*", bool recursive = true)
        {
            return Directory.GetFiles(dirPath, pattern, recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
        }

        public Stream OpenRead(string filePath)
        {
            return File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        }
    }
}
