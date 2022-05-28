using System;

namespace GraphInheritenceTests.ComplexModels
{
    public abstract class NotesBase : IdBase
    {
        public string Text { get; set; }

        public DateTime Date { get; set; }
    }
}