using System.ComponentModel.DataAnnotations;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.DTOs
{
    public abstract class IdBaseDTO : ConcurrencyStampBaseDTO
    {
        [Key]
        public int Id { get; set; }
    }
}