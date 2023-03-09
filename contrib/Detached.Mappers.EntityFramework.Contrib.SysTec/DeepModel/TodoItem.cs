using Detached.Annotations;
using Detached.Mappers.EntityFramework.Contrib.SysTec.ComplexModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.DeepModel
{
    public class TodoItem : IdBase
    {
        public string Title { get; set; }
            
        public bool IsDone { get; set; }

        [Composition]
        public User User { get; set; }

        [Composition]
        public List<ReusedLinkedItem> ReusedLinkedItems { get; set; } = new();
    }
}
