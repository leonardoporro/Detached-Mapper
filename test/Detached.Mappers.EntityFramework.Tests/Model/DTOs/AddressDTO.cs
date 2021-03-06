﻿using Detached.Annotations;

namespace Detached.Mappers.EntityFramework.Tests.Model.DTOs
{
    public class AddressDTO
    {
        public virtual int Id { get; set; }

        public virtual string Street { get; set; }

        public virtual string Number { get; set; }
    }
}