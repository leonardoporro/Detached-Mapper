using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFrameworkCore.Detached.Plugins.ManyToManyPatch
{
    /// <summary>
    /// Provides metadata about many to many properties.
    /// </summary>
    public class ManyToManyNavigation
    {
        public ManyToManyNavigationEnd End1 { get; set; }

        public ManyToManyNavigationEnd End2 { get; set; }

        public EntityType IntermediateEntityType { get; set; }
    }

    public class ManyToManyNavigationEnd
    {
        public EntityType EntityType { get; set; }

        public string PropertyName { get; set; }

        public IClrPropertyGetter Getter { get; set; }

        public IClrPropertySetter Setter { get; set; }
    }
}
