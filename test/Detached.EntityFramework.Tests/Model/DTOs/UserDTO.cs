using System.Collections.Generic;

namespace Detached.EntityFramework.Tests.Model.DTOs
{
    public class UserDTO
    {
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual List<RoleDTO> Roles { get; set; }

        public virtual List<AddressDTO> Addresses { get; set; }

        public virtual UserTypeDTO UserType { get; set; }

        public virtual UserProfileDTO Profile { get; set; }
    }
}