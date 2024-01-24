using System.Collections.Generic;
using Detached.Mappers.EntityFramework.Contrib.SysTec.ComplexModels.inheritance.BaseModel;
using Detached.Mappers.EntityFramework.Contrib.SysTec.Dtos.inheritance.BaseModel;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.Dtos.inheritance;

public class EntityFourDto : BaseStationOneFirstDto
{
    public List<BaseStationOneSecondDto> BaseStationOneSeconds { get; set; } = new();
    
    public List<EntityThreeDto> EntityThrees { get; set; } = new();
}