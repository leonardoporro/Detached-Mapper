using Detached.Mappers.EntityFramework.Contrib.SysTec.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphInheritenceTests.Dtos
{
    public class CountryDto : IdBaseDto
    {
        public string Name { get; set; }
        public string IsoCode { get; set; }

        public int? FlagPictureId { get; set; }
        public PictureDto FlagPicture { get; set; }
    }
}
