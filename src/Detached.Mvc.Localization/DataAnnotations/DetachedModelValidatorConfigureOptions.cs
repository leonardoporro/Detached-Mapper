using Detached.Mvc.Localization.Mapping;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.DataAnnotations.Internal;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Linq;

namespace Detached.Mvc.Localization.DataAnnotations
{
    public class DetachedModelValidatorConfigureOptions : IConfigureOptions<MvcOptions>
    {
        readonly IStringLocalizerFactory _stringLocalizerFactory;
        readonly IValidationAttributeAdapterProvider _validationAttributeAdapterProvider;
        readonly IResourceMapper _resourceMapper;

        public DetachedModelValidatorConfigureOptions(IResourceMapper resourceMapper, IStringLocalizerFactory stringLocalizerFactory, IValidationAttributeAdapterProvider validationAttributeAdapterProvider)
        {
            _stringLocalizerFactory = stringLocalizerFactory;
            _resourceMapper = resourceMapper;
            _validationAttributeAdapterProvider = validationAttributeAdapterProvider;
        }

        public void Configure(MvcOptions options)
        {
            for (int i = 0; i < options.ModelValidatorProviders.Count; i++)
            {
                if (options.ModelValidatorProviders[i] is DataAnnotationsModelValidatorProvider)
                {
                    options.ModelValidatorProviders.RemoveAt(i);
                }
            }

            options.ModelValidatorProviders.Add(new DetachedModelValidatorProvider(_resourceMapper, _validationAttributeAdapterProvider, _stringLocalizerFactory));
        }
    }
}
