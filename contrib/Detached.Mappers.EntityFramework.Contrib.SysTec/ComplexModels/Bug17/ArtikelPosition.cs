using Detached.Annotations;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.ComplexModels.Bug17;

public class ArtikelPosition : PositionBase
{
    [Aggregation]
    public UeberschriftPosition ParentPosition { get; set; }
}