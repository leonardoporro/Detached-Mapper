using Detached.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Detached.Mappers.EntityFramework.Contrib.SysTec.ComplexModels
{
    public class OrganizationNotes : NotesBase
    {
        //Back-references can't be filled in the model
        [Required]
        public int OrganizationId { get; set; }

        [Parent]
        [Required]
        public OrganizationBase Organization { get; set; }
    }
}