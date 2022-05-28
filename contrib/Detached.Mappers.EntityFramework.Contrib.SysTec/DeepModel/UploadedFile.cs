using GraphInheritenceTests.ComplexModels;

namespace GraphInheritenceTests.DeepModel
{
    public class UploadedFile : IdBase
    {
        public string FileTitle { get; set; }
        public bool IsShared { get; set; }
    }
}