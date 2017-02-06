using Detached.Mvc.Localization.Mapping;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace Detached.Mvc.Localization.DataAnnotations
{
    public class DetachedDisplayMetadataConfigureOptions : IConfigureOptions<MvcOptions>
    {
        IStringLocalizerFactory _stringLocalizer;
        IResourceMapper _resourceMapper;

        public DetachedDisplayMetadataConfigureOptions(IResourceMapper resourceMapper, IStringLocalizerFactory stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
            _resourceMapper = resourceMapper;
        }

        public void Configure(MvcOptions options)
        {
            options.ModelMetadataDetailsProviders.Add(new DetachedDisplayMetadataProvider(_resourceMapper, _stringLocalizer));
        }
    }
}
