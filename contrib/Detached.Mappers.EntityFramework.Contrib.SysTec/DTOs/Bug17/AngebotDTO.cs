using System.Collections.Generic;
using Detached.Mappers.EntityFramework.Contrib.SysTec.ComplexModels;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.Dtos.Bug17;

public class AngebotDto : IdBaseDto
{
    public List<PositionBaseDto> Positionen { get; set; }
}