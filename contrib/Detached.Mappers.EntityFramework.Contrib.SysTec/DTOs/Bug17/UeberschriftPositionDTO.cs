using System.Collections.Generic;
using Detached.Annotations;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.DTOs.Bug17;

public class UeberschriftPositionDTO : PositionBaseDTO
{
    [Composition]
    public List<PositionBaseDTO> Positionen { get; set; }
}