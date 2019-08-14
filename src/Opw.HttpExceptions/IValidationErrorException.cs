using System.Collections.Generic;

namespace Opw.HttpExceptions
{
    /// <summary>
    /// Interface for ValidationErrorExceptions.
    /// </summary>
    internal interface IValidationErrorException
    {
        /// <summary>
        /// The results of validation.
        /// </summary>
        IDictionary<string, object[]> GetErrors();
    }
}