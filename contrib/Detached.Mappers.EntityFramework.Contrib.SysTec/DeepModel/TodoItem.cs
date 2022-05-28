using Detached.Annotations;
using GraphInheritenceTests.ComplexModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphInheritenceTests.DeepModel
{
    public class TodoItem : IdBase
    {
        public string Title { get; set; }

        public bool IsDone { get; set; }

        [Composition]
        public List<SubTodoItem> SubTodoItems { get; set; } = new List<SubTodoItem>();
    }
}
