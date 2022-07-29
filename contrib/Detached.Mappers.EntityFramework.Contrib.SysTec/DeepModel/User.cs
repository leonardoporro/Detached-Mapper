using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Detached.Annotations;
using GraphInheritenceTests.ComplexModels;

namespace GraphInheritenceTests.DeepModel
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
