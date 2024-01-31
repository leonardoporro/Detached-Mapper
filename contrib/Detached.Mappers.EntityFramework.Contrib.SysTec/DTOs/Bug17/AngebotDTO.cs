using System.Collections.Generic;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.Dtos.Bug17;

public class AngebotDto : IdBaseDto
{
    public List<PositionBaseDto> Positionen { get; set; }
}