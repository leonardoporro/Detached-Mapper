using System.Collections.Generic;
using Detached.Mappers.EntityFramework.Contrib.SysTec.ComplexModels.inheritance.BaseModel;
using Detached.Mappers.EntityFramework.Contrib.SysTec.Dtos.inheritance.BaseModel;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.Dtos.inheritance;

public class EntityOneDto : IdBaseDto
{
    public List<BaseHeadDto> BaseHeads { get; set; } = new();
}