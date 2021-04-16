using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Detached.Mappers.EntityFramework.Tests.Model.DTOs
{
    public class CustomerDTO
    {
        public Guid? Id { get; set; }

        public string Name { get; set; }
    }
}
