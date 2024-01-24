using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.Dtos
{
    public abstract class IdBaseDto : ConcurrencyStampBaseDto
    {
        [Key]
        public int Id { get; set; }
    }
}
