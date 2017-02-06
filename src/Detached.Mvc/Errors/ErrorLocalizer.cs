using Detached.Mvc.Localization.Mapping;
using Detached.Mvc.Validation;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Detached.Mvc.Localization.Errors
{
    public class ErrorLocalizer : IErrorLocalizer
    {
        IStringLocalizerFactory _stringLocalizerFactory;
        IResourceMapper _resourceMapper;

        public ErrorLocalizer(IStringLocalizerFactory stringLocalizerFactory,
                              IResourceMapper resourceMapper)
        {
            _stringLocalizerFactory = stringLocalizerFactory;
            _resourceMapper = resourceMapper;
        }

        public LocalizedString GetLocalizedErrorMessage(ControllerActionDescriptor controllerAction, string errorCode)
        {
            LocalizedString errorMessage;
            ResourceKey resourceKey = _resourceMapper.GetKey(controllerAction.ControllerTypeInfo.Namespace,
                                                            controllerAction.ControllerTypeInfo.Name,
                                                            controllerAction.ActionName,
                                                            errorCode);
            if (resourceKey != null)
            {
                IStringLocalizer stringLocalizer = _stringLocalizerFactory.Create(resourceKey.ResourceName, resourceKey.ResourceLocation);
                errorMessage = stringLocalizer.GetString(resourceKey.KeyName, _resourceMapper.GetSupportInfoArguments());
                if (errorMessage.ResourceNotFound)
                {
                    errorMessage = GetFallbackErrorMessage(errorCode);
                }
            }
            else
            {
                errorMessage = GetFallbackErrorMessage(errorCode);
            }

            return errorMessage;
        }

        public LocalizedString GetFallbackErrorMessage(string errorCode)
        {
            ResourceKey resourceKey = _resourceMapper.GetFallbackKey("Errors", errorCode, nameof(ModelError.ErrorMessage));
            if (resourceKey != null)
            {
                IStringLocalizer stringLocalizer = _stringLocalizerFactory.Create(resourceKey.ResourceName, resourceKey.ResourceLocation);
                return stringLocalizer.GetString(resourceKey.KeyName, _resourceMapper.GetSupportInfoArguments());
            }
            else
            {
                return new LocalizedString(errorCode, errorCode, true);
            }
        }
    }
}
