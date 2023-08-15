using System.Collections.Generic;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.DTOs.Bug17;

public class AngebotDTO : IdBaseDTO
{
    public List<PositionBaseDTO> Positionen { get; set; }
}