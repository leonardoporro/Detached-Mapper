using System.Collections.Generic;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.DTOs
{
    public class CourseDTO : IdBaseDTO
    {
        public string CourseName { get; set; }

        public int ClassRoomNumber { get; set; }

        public List<StudentDTO> Students { get; set; }
    }
}
