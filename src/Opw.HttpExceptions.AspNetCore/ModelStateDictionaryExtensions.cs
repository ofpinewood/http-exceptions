using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;

namespace Opw.HttpExceptions.AspNetCore
{
    internal static class ModelStateDictionaryExtensions
    {
        internal static IDictionary<string, string[]> ToDictionary(this ModelStateDictionary modelState)
        {
            var details = new ValidationProblemDetails(modelState);
            return details.Errors;
        }
    }
}
