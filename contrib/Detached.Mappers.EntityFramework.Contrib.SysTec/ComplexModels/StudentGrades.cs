using Detached.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.ComplexModels;

[Owned]
public class StudentGrades
{
    public string English { get; set; }

    public string Math { get; set; }

    public string ComputerScience { get; set; }
}