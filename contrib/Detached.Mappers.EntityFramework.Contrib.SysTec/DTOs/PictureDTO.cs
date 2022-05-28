using GraphInheritenceTests.ComplexModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphInheritenceTests.DTOs
{
    public class PictureDTO : IdBase
    {
        public string FileName { get; set; }
    }
}