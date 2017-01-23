using Detached.Mvc.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace Detached.Mvc.Localization
{
    public class LocalizedMetadataProviderSetup : IConfigureOptions<MvcOptions>
    {
        IStringLocalizerFactory _stringLocalizer;
        IMetadataProvider _metadataProvider;

        public LocalizedMetadataProviderSetup(IStringLocalizerFactory stringLocalizer, IMetadataProvider metadataProvider)
        {
            _stringLocalizer = stringLocalizer;
            _metadataProvider = metadataProvider;
        }

        public void Configure(MvcOptions options)
        {
            options.ModelMetadataDetailsProviders.Add(new LocalizedMetadataProvider(_stringLocalizer, _metadataProvider));
        }
    }
}
