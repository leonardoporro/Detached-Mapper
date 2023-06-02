using System.Collections.Generic;
using Detached.Mappers.EntityFramework.Contrib.SysTec.ComplexModels.inheritance.BaseModel;
using Detached.Mappers.EntityFramework.Contrib.SysTec.DTOs.inheritance.BaseModel;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.DTOs.inheritance;

public class EntityFourDTO : BaseStationOneFirstDTO
{
    public List<BaseStationOneSecondDTO> BaseStationOneSeconds { get; set; } = new();
    
    public List<EntityThreeDTO> EntityThrees { get; set; } = new();
}