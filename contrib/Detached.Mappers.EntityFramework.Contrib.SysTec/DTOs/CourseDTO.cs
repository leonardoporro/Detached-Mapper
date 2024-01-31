using System.Collections.Generic;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.Dtos
{
    public class CourseDto : IdBaseDto
    {
        public string CourseName { get; set; }

        public int ClassRoomNumber { get; set; }

        public List<StudentDto> Students { get; set; }
    }
}
