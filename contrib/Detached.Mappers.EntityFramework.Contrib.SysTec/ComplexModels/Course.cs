using Detached.Annotations;
using System.Collections.Generic;

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
