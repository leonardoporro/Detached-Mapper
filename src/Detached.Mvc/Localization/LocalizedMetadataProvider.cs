using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System;
using System.Reflection;

namespace Detached.Mvc.Metadata
{
    public class LocalizedMetadataProvider : IDisplayMetadataProvider, IValidationMetadataProvider, IBindingMetadataProvider
    {
        IStringLocalizerFactory _stringLocalizerFactory;
        IMetadataProvider _metadataProvider;

        public LocalizedMetadataProvider(IStringLocalizerFactory stringLocalizerFactory, IMetadataProvider metadataProvider)
        {
            _stringLocalizerFactory = stringLocalizerFactory;
            _metadataProvider = metadataProvider;
        }

        public string TypeTemplate { get; set; } = "{module}.{feature}.{class}";

        public string PropertyTemplate { get; set; } = "{module}.{feature}.{class}.{property}.{metaproperty}";

        public string ValidatorTemplate { get; set; } = "{module}.{feature}.{class}.{property}";

        public void CreateDisplayMetadata(DisplayMetadataProviderContext context)
        {
            IStringLocalizer stringLocalizer;
            switch (context.Key.MetadataKind)
            {
                case ModelMetadataKind.Property:
                    stringLocalizer = _stringLocalizerFactory.Create(context.Key.ContainerType);
                    string displayKey = _metadataProvider.Resolve(PropertyTemplate, context.Key.ContainerType, context.Key.Name, "DisplayName");
                    if (displayKey != null)
                        context.DisplayMetadata.DisplayName = () => stringLocalizer.GetString(displayKey);

                    string placeholderKey = _metadataProvider.Resolve(PropertyTemplate, context.Key.ContainerType, context.Key.Name, "Placeholder");
                    if (placeholderKey != null)
                        context.DisplayMetadata.Placeholder = () => stringLocalizer.GetString(placeholderKey);
                    break;
                case ModelMetadataKind.Type:
                    stringLocalizer = _stringLocalizerFactory.Create(context.Key.ModelType);
                    string dataTypeKey = _metadataProvider.Resolve(PropertyTemplate, context.Key.ModelType);
                    if (dataTypeKey != null)
                        context.DisplayMetadata.DisplayName = () => stringLocalizer.GetString(dataTypeKey);
                    break;
            }
        }

        public void CreateValidationMetadata(ValidationMetadataProviderContext context)
        {
            foreach (ValidationAttribute attribute in context.ValidationMetadata
                                                             .ValidatorMetadata
                                                             .OfType<ValidationAttribute>())
            {
                if (attribute.ErrorMessage == null)
                {
                    IStringLocalizer stringLocalizer = _stringLocalizerFactory.Create(attribute.GetType());
                    string key = _metadataProvider.Resolve(ValidatorTemplate, attribute.GetType(), nameof(ValidationAttribute.ErrorMessage));
                    if (key != null)
                        attribute.ErrorMessage = stringLocalizer.GetString(key);
                }
            }
        }

        public void CreateBindingMetadata(BindingMetadataProviderContext context)
        {
        }
    }
}
