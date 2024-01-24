using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Detached.Mappers.EntityFramework.Contrib.SysTec.ComplexModels;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.Dtos
{
    public class StudentDto : IdBaseDto
    {
        public string Name { get; set; }
        
        public int Age { get; set; }
        
        public StudentGrades Grades { get; set; }
    }
}
