using Detached.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations;

namespace Detached.EntityFramework.Tests.Plugins.Auditing
{
    public class EntityForAuditing
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
