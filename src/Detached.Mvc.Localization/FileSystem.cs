using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Detached.Mvc
{
    public class FileSystem : IFileSystem
    {
        public bool Exists(string filePath)
        {
            return File.Exists(filePath);
        }

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
