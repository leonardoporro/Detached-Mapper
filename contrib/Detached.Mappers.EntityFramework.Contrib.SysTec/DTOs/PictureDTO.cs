using Detached.Mappers.EntityFramework.Contrib.SysTec.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphInheritenceTests.Dtos
{
    public class PictureDto : IdBaseDto
    {
        public string FileName { get; set; }
    }
}