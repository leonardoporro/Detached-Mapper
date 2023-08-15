using Detached.Mappers.EntityFramework.Contrib.SysTec.ComplexModels;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.DTOs
{
    public class StudentDTO : IdBaseDTO
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public StudentGrades Grades { get; set; }
    }
}