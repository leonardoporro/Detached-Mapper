using System;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.ComplexModels
{
    public abstract class NotesBase : IdBase
    {
        public string Text { get; set; }

        public DateTime Date { get; set; }
    }
}