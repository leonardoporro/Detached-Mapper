using Detached.Mvc.Localization.Mapping;
using Detached.Mvc.Validation;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Localization;

namespace Detached.Mvc.Localization.Errors
{
    /// <summary>
    /// Provides automatically localized error/validation messages using IResourceMapper 
    /// and IStringLocalizerFactory.
    /// </summary>
    public class ErrorLocalizer : IErrorLocalizer
    {
        IStringLocalizerFactory _stringLocalizerFactory;
        IResourceMapper _resourceMapper;

        /// <summary>
        /// Creates a new instance of ErrorLocalizer.
        /// </summary>
        /// <param name="stringLocalizerFactory">A valid instance of a localizer.</param>
        /// <param name="resourceMapper">A configured resource mapper.</param>
        public ErrorLocalizer(IStringLocalizerFactory stringLocalizerFactory,
                              IResourceMapper resourceMapper)
        {
            _stringLocalizerFactory = stringLocalizerFactory;
            _resourceMapper = resourceMapper;
        }

        /// <summary>
        /// Gets a translated error message for the given controller action.
        /// </summary>
        /// <param name="controllerAction">The controller action that originated the error.</param>
        /// <param name="errorCode">The code of the error to translate.</param>
        /// <returns>A localized string.</returns>
        public LocalizedString GetLocalizedErrorMessage(ControllerActionDescriptor controllerAction, string errorCode)
        {
            LocalizedString errorMessage;
            // build a key for the resource.
            ResourceKey resourceKey = _resourceMapper.GetKey(controllerAction.ControllerTypeInfo.Namespace,
                                                            controllerAction.ControllerTypeInfo.Name,
                                                            controllerAction.ActionName,
                                                            errorCode);
            if (resourceKey != null)
            {
                // if the key was built, proceed to translation.
                IStringLocalizer stringLocalizer = _stringLocalizerFactory.Create(resourceKey.ResourceName, resourceKey.ResourceLocation);
                errorMessage = stringLocalizer.GetString(resourceKey.KeyName, _resourceMapper.GetSupportInfoArguments());
                if (errorMessage.ResourceNotFound)
                {
                    errorMessage = GetFallbackErrorMessage(errorCode);
                }
            }
            else
            {
                // the key was not built, get the default error for the message.
                errorMessage = GetFallbackErrorMessage(errorCode);
            }

            return errorMessage;
        }

        /// <summary>
        /// Gets a generic error message, in case the controller-specific error message was not defined.
        /// </summary>
        /// <param name="errorCode">The error code whose generic message is required.</param>
        /// <returns>A localized string.</returns>
        public LocalizedString GetFallbackErrorMessage(string errorCode)
        {
            ResourceKey resourceKey = _resourceMapper.GetFallbackKey("Errors", errorCode, nameof(ApiModelError.Message));
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
