using Detached.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
