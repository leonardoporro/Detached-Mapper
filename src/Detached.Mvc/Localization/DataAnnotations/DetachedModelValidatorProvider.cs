using Detached.Mvc.Localization.Mapping;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.DataAnnotations.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Detached.Mvc.Localization.DataAnnotations
{
    public class DetachedModelValidatorProvider : IModelValidatorProvider
    {
        readonly IStringLocalizerFactory _stringLocalizerFactory;
        readonly IValidationAttributeAdapterProvider _validationAttributeAdapterProvider;
        readonly IResourceMapper _resourceMapper;

        public DetachedModelValidatorProvider(IResourceMapper resourceMapper,  IValidationAttributeAdapterProvider validationAttributeAdapterProvider, IStringLocalizerFactory stringLocalizerFactory)
        {
            _validationAttributeAdapterProvider = validationAttributeAdapterProvider;
            _stringLocalizerFactory = stringLocalizerFactory;
            _resourceMapper = resourceMapper;
        }

        public void CreateValidators(ModelValidatorProviderContext context)
        {
            for (var i = 0; i < context.Results.Count; i++)
            {
                var validatorItem = context.Results[i];
                if (validatorItem.Validator != null)
                {
                    continue;
                }

                var attribute = validatorItem.ValidatorMetadata as ValidationAttribute;
                if (attribute == null)
                {
                    continue;
                }

                IStringLocalizer stringLocalizer = null;
                Type modelType = context.ModelMetadata.ContainerType ?? context.ModelMetadata.ModelType;
                string validatorName = attribute.GetType().Name.Replace("Attribute", "");
                ResourceKey key = _resourceMapper.GetKey(modelType.Namespace,
                                                         modelType.Name,
                                                         context.ModelMetadata.PropertyName,
                                                         validatorName);
                if (key != null)
                {
                    stringLocalizer = _stringLocalizerFactory.Create(key.ResourceName, key.ResourceLocation);
                    if (attribute.ErrorMessage == null)
                        attribute.ErrorMessage = key.KeyName;
                }

                ResourceKey alternateKey = _resourceMapper.GetFallbackKey("Validation", validatorName, nameof(ValidationAttribute.ErrorMessage));
                if (alternateKey != null)
                {
                    IStringLocalizer alternateStringLocalizer = _stringLocalizerFactory.Create(alternateKey.ResourceName, alternateKey.ResourceLocation);
                    stringLocalizer = new ValidatorStringLocalizerAdapter(new[] { stringLocalizer, alternateStringLocalizer }, new[] { key.KeyName, alternateKey.KeyName });
                }

                if (stringLocalizer == null)
                    stringLocalizer = _stringLocalizerFactory.Create(context.ModelMetadata.ContainerType ?? context.ModelMetadata.ModelType);

                var validator = new DataAnnotationsModelValidator(_validationAttributeAdapterProvider, attribute, stringLocalizer);
                validatorItem.Validator = validator;
                validatorItem.IsReusable = true;

                if (attribute is RequiredAttribute)
                {
                    context.Results.Remove(validatorItem);
                    context.Results.Insert(0, validatorItem);
                }
            }

            if (typeof(IValidatableObject).IsAssignableFrom(context.ModelMetadata.ModelType))
            {
                context.Results.Add(new ValidatorItem
                {
                    Validator = new ValidatableObjectAdapter(),
                    IsReusable = true
                });
            }
        }
    }
}

