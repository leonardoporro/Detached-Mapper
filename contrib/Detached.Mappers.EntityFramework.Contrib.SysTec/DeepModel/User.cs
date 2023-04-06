using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Detached.Annotations;
using Detached.Mappers.EntityFramework.Contrib.SysTec.ComplexModels;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.DeepModel
{
    public class User: IdBase
    {
        public string Name { get; set; }

        [Composition]
        public List<ReusedLinkedItem> ReusedLinkedItems { get; set; } = new();

        [Aggregation]
        public List<TodoItem> TodoItems { get; set; } = new();
    }
}
