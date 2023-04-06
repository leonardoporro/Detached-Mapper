using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.DTOs
{
    public class CourseDTO : IdBaseDTO
    {
        public string CourseName { get; set; }

        public int ClassRoomNumber { get; set; }

        public List<StudentDTO> Students { get; set; }
    }
}
