using Detached.Mappers.EntityFramework.Contrib.SysTec.ComplexModels;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.DeepModel
{
    public class UploadedFile : IdBase
    {
        public string FileTitle { get; set; }
        public bool IsShared { get; set; }
    }
}