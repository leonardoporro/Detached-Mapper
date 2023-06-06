using Detached.Mappers.EntityFramework.Contrib.SysTec.ComplexModels;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.DTOs.Bug17;

public abstract class PositionBaseDTO : IdBaseDTO
{
    public string Positionsart { get; set; }
}