using Detached.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraphInheritenceTests.ComplexModels
{
    public class OrganizationNotes : NotesBase
    {
        //Back-references can't be filled in the model
        [Required]
        [NotMapped]
        public int OrganizationId { get; set; }

        [Parent]
        [Required]
        public OrganizationBase Organization { get; set; }
    }
}