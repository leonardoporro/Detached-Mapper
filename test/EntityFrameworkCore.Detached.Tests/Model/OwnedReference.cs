using EntityFrameworkCore.Detached.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Tests.Model
{
    public class OwnedReference
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
