using System.Collections.Generic;

namespace Detached.EntityFramework.Tests.Model.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<RoleDTO> Roles { get; set; }

        public List<AddressDTO> Addresses { get; set; }

        public UserTypeDTO UserType { get; set; }

        public UserProfileDTO Profile { get; set; }
    }
}