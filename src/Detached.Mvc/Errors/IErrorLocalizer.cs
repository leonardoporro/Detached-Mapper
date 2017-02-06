using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Localization;

namespace Detached.Mvc.Localization.Errors
{
    public interface IErrorLocalizer
    {
        LocalizedString GetLocalizedErrorMessage(ControllerActionDescriptor controllerAction, string errorCode);
    }
}