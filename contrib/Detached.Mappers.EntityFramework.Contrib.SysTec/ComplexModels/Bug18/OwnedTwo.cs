using Microsoft.EntityFrameworkCore;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.ComplexModels.Bug18;

[Owned]
public class OwnedTwo
{
    public bool Bool { get; set; }
}