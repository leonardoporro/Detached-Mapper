using Detached.Annotations;
using Detached.Mappers.EntityFramework.Contrib.SysTec.ComplexModels;
using System.Collections.Generic;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.DeepModel
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