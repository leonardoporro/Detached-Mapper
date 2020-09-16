﻿using Detached.Annotations;

namespace Detached.EntityFramework.Tests.Model.DTOs
{
    public class UserProfileDTO
    {
        public virtual int Id { get; set; }

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }
    }
}