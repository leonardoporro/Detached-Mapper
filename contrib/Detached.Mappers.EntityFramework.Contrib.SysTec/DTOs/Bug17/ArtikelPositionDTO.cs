using Detached.Annotations;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.Dtos.Bug17;

public class ArtikelPositionDto : PositionBaseDto
{
    [Aggregation]
    public UeberschriftPositionDto ParentPosition { get; set; }
}