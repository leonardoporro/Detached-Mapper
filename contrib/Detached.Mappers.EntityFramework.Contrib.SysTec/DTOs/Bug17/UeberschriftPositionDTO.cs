using System.Collections.Generic;
using Detached.Annotations;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.Dtos.Bug17;

public class UeberschriftPositionDto : PositionBaseDto
{
    [Composition]
    public List<PositionBaseDto> Positionen { get; set; }
}