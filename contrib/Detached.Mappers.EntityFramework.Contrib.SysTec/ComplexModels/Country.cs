using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphInheritenceTests.ComplexModels
{
    public class Country : IdBase
    {
        public string Name { get; set; }
        public string IsoCode { get; set; }

        public int? FlagPictureId { get; set; }
        public Picture FlagPicture { get; set; }
    }
}
