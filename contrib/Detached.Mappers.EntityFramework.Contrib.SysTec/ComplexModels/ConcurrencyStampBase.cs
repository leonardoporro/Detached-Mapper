using System.ComponentModel.DataAnnotations;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.ComplexModels;

public abstract class ConcurrencyStampBase
{
    [ConcurrencyCheck]
    public int ConcurrencyToken { get; set; }
}