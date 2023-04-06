using Detached.Annotations;
using Detached.Mappers.EntityFramework.Contrib.SysTec.ComplexModels;
using System.Collections.Generic;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.DeepModel
{
    public class SubTodoItem : IdBase
    {
        public string Title { get; set; }

        [Composition]
        public List<UploadedFile> UploadedFiles { get; set; }
    }
}