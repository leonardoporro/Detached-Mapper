using Detached.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.ComplexModels
{
    public class Course : IdBase
    {
        public string CourseName { get; set; }

        public int ClassRoomNumber { get; set; }

        [Aggregation]
        public List<Student> Students { get; set; }
    }
}
