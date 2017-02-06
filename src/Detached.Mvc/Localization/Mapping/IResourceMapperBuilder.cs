using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Detached.Mvc.Localization.Mapping
{
    public interface IResourceMapperBuilder
    {
        IServiceCollection Services { get; }
    }

    public class ResourceMapperBuilder : IResourceMapperBuilder
    {
        public IServiceCollection Services { get; set; }
    }
}
