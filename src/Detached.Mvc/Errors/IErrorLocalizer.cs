using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Localization;

namespace Detached.Mvc.Localization.Errors
{
    public interface IErrorLocalizer
    {
        /// <summary>
        /// Gets a translated error message for the given controller action.
        /// </summary>
        /// <param name="controllerAction">The controller action that originated the error.</param>
        /// <param name="errorCode">The code of the error to translate.</param>
        /// <returns>A localized string.</returns>
        LocalizedString GetLocalizedErrorMessage(ControllerActionDescriptor controllerAction, string errorCode);
    }
}