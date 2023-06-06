using System.Collections.Generic;
using Detached.Annotations;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.ComplexModels.Bug17;

public class UeberschriftPosition : PositionBase
{
    [Composition]
    public List<PositionBase> Positionen { get; set; }
}