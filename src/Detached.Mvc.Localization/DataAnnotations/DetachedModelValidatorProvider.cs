using Detached.Mvc.Localization.Mapping;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.DataAnnotations.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Globalization;

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
                ResourceKey key = _resourceMapper.GetFieldKey(context.ModelMetadata.ContainerType ?? context.ModelMetadata.ModelType,
                                                              context.ModelMetadata.PropertyName,
                                                              attribute.GetType().Name.Replace("Attribute", ""));
                if (key != null)
                {
                    stringLocalizer = _stringLocalizerFactory.Create(key.Source, key.Location);
                    if (attribute.ErrorMessage == null)
                        attribute.ErrorMessage = key.Name;
                }

                ResourceKey alternateKey = _resourceMapper.GetFieldKey(attribute.GetType(), "ErrorMessage");
                if (alternateKey != null)
                {
                    IStringLocalizer alternateStringLocalizer = _stringLocalizerFactory.Create(alternateKey.Source, alternateKey.Location);
                    stringLocalizer = new CompositeStringLocalizer(new[] { stringLocalizer, alternateStringLocalizer }, new[] { key.Name, alternateKey.Name });
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

