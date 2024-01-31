using System.ComponentModel.DataAnnotations;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.Dtos
{
    public abstract class IdBaseDto : ConcurrencyStampBaseDto
    {
        [Key]
        public int Id { get; set; }
    }
}