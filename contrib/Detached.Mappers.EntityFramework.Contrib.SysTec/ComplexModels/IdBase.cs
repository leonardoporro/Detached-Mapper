using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.ComplexModels
{
    public abstract class IdBase
    {
        [Key]
        public int Id { get; set; }

        //[Required]
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        //public DateTime CreatedAt { get; set; }

        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        //public DateTime? ModifiedAt { get; set; }
    }
}