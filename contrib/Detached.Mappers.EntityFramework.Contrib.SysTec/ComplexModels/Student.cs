using Detached.Annotations;
using System.Collections.Generic;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.ComplexModels
{
    public class Student : IdBase
    {
        public string Name { get; set; }

        public int Age { get; set; }

        [Aggregation]
        public List<Course> Courses { get; set; }
    }
}
