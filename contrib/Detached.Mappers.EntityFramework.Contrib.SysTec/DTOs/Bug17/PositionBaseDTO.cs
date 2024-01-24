using Detached.Mappers.EntityFramework.Contrib.SysTec.ComplexModels;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.Dtos.Bug17;

public abstract class PositionBaseDto : IdBaseDto
{
    public string Positionsart { get; set; }
}