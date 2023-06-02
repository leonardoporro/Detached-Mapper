using System.Collections.Generic;
using Detached.Mappers.EntityFramework.Contrib.SysTec.ComplexModels.inheritance.BaseModel;
using Detached.Mappers.EntityFramework.Contrib.SysTec.DTOs.inheritance.BaseModel;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.DTOs.inheritance;

public class EntityOneDTO : IdBaseDTO
{
    public List<BaseHeadDTO> BaseHeads { get; set; } = new();
}