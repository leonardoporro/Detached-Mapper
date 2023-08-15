using Detached.Mappers.EntityFramework.Contrib.SysTec.DTOs;

namespace GraphInheritenceTests.DTOs
{
    public class PictureDTO : IdBaseDTO
    {
        public string FileName { get; set; }
    }
}