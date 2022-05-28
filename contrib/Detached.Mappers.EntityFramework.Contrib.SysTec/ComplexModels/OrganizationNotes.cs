using Detached.Annotations;
using System.ComponentModel.DataAnnotations;

namespace GraphInheritenceTests.ComplexModels
{
    public class OrganizationNotes : NotesBase
    {
        // Back-references can't be filled in the model
        [Required]
        public int OrganizationId { get; set; }

        [Required]
        public OrganizationBase Organization { get; set; }
    }
}