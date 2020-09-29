using Detached.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Detached.Mappers.EntityFramework.Tests.Model
{
    public class User
    {
        [Key]
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual DateTime DateOfBirth { get; set; }

        public virtual List<Role> Roles { get; set; }

        [Composition]
        public virtual List<Address> Addresses { get; set; }

        public virtual UserType UserType { get; set; }

        [Composition]
        public virtual UserProfile Profile { get; set; }
    }
}