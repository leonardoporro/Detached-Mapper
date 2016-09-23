using EntityFrameworkCore.Detached.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Demo.Model
{
    public class Company
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        [Owned]
        public IList<SellPoint> SellPoints { get; set; }

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
