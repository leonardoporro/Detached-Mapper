using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Detached.Mvc.Metadata
{
    public class LocalizedMetadataProvider : IDisplayMetadataProvider, IValidationMetadataProvider, IBindingMetadataProvider
    {
        IStringLocalizerFactory _stringLocalizerFactory;
        IMetadataProvider _metadataProvider;

        public LocalizedMetadataProvider(IStringLocalizerFactory stringLocalizerFactory)
        {
            _stringLocalizerFactory = stringLocalizerFactory;
        }

        public void CreateDisplayMetadata(DisplayMetadataProviderContext context)
        {
            switch (context.Key.MetadataKind)
            {
                case ModelMetadataKind.Property:
                    break;
                case ModelMetadataKind.Type:
                    break;
            }
        }

        public void CreateValidationMetadata(ValidationMetadataProviderContext context)
        {
            foreach (ValidationAttribute attribute in context.ValidationMetadata
                                                             .ValidatorMetadata
                                                             .OfType<ValidationAttribute>())
            {
            }
        }

        public void CreateBindingMetadata(BindingMetadataProviderContext context)
        {
        }
    }
}
