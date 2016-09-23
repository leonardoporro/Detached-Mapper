using EntityFrameworkCore.Detached.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Tests.Model.Audit
{
    public class EntityForAudit
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        [CreatedBy]
        public string CreatedBy { get; set; }

        [CreatedDate]
        public DateTime CreatedDate { get; set; }

        [ModifiedBy]
        public string ModifiedBy { get; set; }

        [ModifiedDate]
        public DateTime ModifiedDate { get; set; }
    }
}
