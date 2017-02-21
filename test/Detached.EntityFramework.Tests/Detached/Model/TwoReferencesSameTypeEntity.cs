using Detached.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Detached.EntityFramework.Tests
{
    public class TwoReferencesSameTypeEntity
    {
        [Key]
        public int Id { get; set; }

        [Owned]
        public TwoReferencesSameTypeReference ReferenceA { get; set; }

        [Owned]
        public TwoReferencesSameTypeReference ReferenceB { get; set; }

        [Owned]
        public TwoReferencesSameTypeReference ReferenceC { get; set; }
    }

    public class TwoReferencesSameTypeReference
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        [Owned]
        public IList<TwoReferencesSameTypeItem> Items { get; set; } = new List<TwoReferencesSameTypeItem>();
    }

    public class TwoReferencesSameTypeItem
    {
        [Key]
        public int Id { get; set; }

        public TwoReferencesSameTypeReference Parent { get; set; }

        public string Name { get; set; }
    }
}
