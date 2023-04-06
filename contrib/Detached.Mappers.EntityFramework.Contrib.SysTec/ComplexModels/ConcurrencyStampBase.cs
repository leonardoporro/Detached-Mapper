using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.ComplexModels;

public abstract class ConcurrencyStampBase
{
    [ConcurrencyCheck]
    public int ConcurrencyToken { get; set; }
}