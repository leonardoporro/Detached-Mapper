using Detached.Annotations;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.DTOs.Bug17;

public class ArtikelPositionDTO : PositionBaseDTO
{
    [Aggregation]
    public UeberschriftPositionDTO ParentPosition { get; set; }
}