using System.Collections.Generic;
using Detached.Annotations;
using Detached.Mappers.EntityFramework.Contrib.SysTec.ComplexModels.inheritance.BaseModel;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.ComplexModels.inheritance;

public class EntityOne : IdBase
{
    [Composition]
    public List<BaseHead> BaseHeads { get; set; } = new();
}