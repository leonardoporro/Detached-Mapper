using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Detached.Mvc
{
    /// <summary>
    /// Provides decoupled access to the file system.
    /// </summary>
    public interface IFileSystem
    {
        /// <summary>
        /// Opens the file and retunrs a stream to read from.
        /// </summary>
        /// <param name="filePath">The path to the file to open.</param>
        Stream OpenRead(string filePath);

        /// <summary>
        /// Returns True if the file at the given path exists.
        /// </summary>
        /// <param name="filePath">The path to the file being verified.</param>
        bool Exists(string filePath);
    }
}
