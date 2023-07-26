using System.Collections.Generic;
using Detached.Mappers.EntityFramework.Contrib.SysTec.ComplexModels;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.DTOs.Bug17;

public class AngebotDTO : IdBaseDTO
{
    public List<PositionBaseDTO> Positionen { get; set; }
}