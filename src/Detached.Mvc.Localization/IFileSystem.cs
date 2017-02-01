using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Detached.Mvc
{
    public interface IFileSystem
    {
        Stream OpenRead(string filePath);

        bool Exists(string filePath);
    }
}
