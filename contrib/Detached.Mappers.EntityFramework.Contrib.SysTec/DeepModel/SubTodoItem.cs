using Detached.Annotations;
using GraphInheritenceTests.ComplexModels;
using System.Collections.Generic;

namespace GraphInheritenceTests.DeepModel
{
    public class SubTodoItem : IdBase
    {
        public string Title { get; set; }

        [Composition]
        public List<UploadedFile> UploadedFiles { get; set; }
    }
}