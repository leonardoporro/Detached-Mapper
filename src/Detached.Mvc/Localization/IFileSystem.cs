using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Detached.Mvc.Localization
{
    public interface IFileSystem
    {
        string[] GetFiles(string dirPath, string pattern = "*", bool recursive = true);
        Stream OpenRead(string filePath);
    }
}
