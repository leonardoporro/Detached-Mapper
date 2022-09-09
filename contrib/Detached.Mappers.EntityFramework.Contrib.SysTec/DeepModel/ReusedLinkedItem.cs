using Detached.Annotations;
using GraphInheritenceTests.ComplexModels;
using System.Collections.Generic;

namespace GraphInheritenceTests.DeepModel
{
    /// <summary>
    /// Will be used multiple times to reproduce a bug
    /// </summary>
    public class ReusedLinkedItem : IdBase
    {
        public string Title { get; set; }

        [Composition]
        public List<UploadedFile> UploadedFiles { get; set; }
    }
}