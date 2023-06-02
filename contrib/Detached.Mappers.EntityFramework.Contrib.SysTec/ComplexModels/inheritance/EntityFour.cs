using System.Collections.Generic;
using Detached.Annotations;
using Detached.Mappers.EntityFramework.Contrib.SysTec.ComplexModels.inheritance.BaseModel;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.ComplexModels.inheritance;

public class EntityFour : BaseStationOneFirst
{
    [Composition]
    public List<BaseStationOneSecond> BaseStationOneSeconds { get; set; } = new();
}