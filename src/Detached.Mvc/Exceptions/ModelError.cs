using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Detached.Mvc.Exceptions
{
    public class ModelError : ApiError
    {
        #region Ctor.

        public ModelError()
        {
            //ErrorCode = "model_validation";
            Details = "Invalid model.";
        }

        public ModelError(ModelStateDictionary modelState)
            : this()
        {
            foreach (var entry in modelState)
            {
                if (entry.Value.Errors.Any())
                {
                    MemberErrors.Add(entry.Key, string.Join(".", entry.Value.Errors.Select(e => e.ErrorMessage)));
                }
            }
        }

        #endregion

        public Dictionary<string, string> MemberErrors { get; set; } = new Dictionary<string, string>();
    }
}