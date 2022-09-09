using Detached.Mappers.EntityFramework.Contrib.SysTec.DTOs;
using GraphInheritenceTests.ComplexModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphInheritenceTests.DTOs
{
    public class PictureDTO : IdBaseDTO
    {
        public string FileName { get; set; }
    }
}