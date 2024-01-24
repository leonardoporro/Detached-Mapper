using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.Dtos
{
    public class CourseDto : IdBaseDto
    {
        public string CourseName { get; set; }

        public int ClassRoomNumber { get; set; }

        public List<StudentDto> Students { get; set; }
    }
}
