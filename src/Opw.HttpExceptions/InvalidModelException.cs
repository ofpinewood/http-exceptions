using System;
using System.Collections.Generic;

namespace Opw.HttpExceptions
{
    /// <summary>
    /// Represents HTTP BadRequest (400) errors because of an invalid model.
    /// </summary>
    public class InvalidModelException : BadRequestException
    {
        /// <summary>
        /// The results of a validation request.
        /// </summary>
        public IDictionary<string, string[]> Errors { get; } = new Dictionary<string, string[]>(StringComparer.Ordinal);

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidModelException"></see> class with status code BadRequest.
        /// </summary>
        /// <param name="errors">The error messages for the member.</param>
        /// <param name="memberName">The member name that indicate which field have an error.</param>
        public InvalidModelException(string memberName, params string[] errors)
        {
            Errors.Add(memberName, errors);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidModelException"></see> class with status code BadRequest.
        /// </summary>
        /// <param name = "errors" > The validation errors.</param>
        public InvalidModelException(IDictionary<string, string[]> errors)
        {
            if (errors == null)
                throw new ArgumentNullException(nameof(errors));

            Errors = new Dictionary<string, string[]>(errors, StringComparer.Ordinal);
        }

        #region private constructors to disable warning RCS1194 Implement exception constructors
#pragma warning disable IDE0051 // Remove unused private members
        private InvalidModelException() : base() { }
        private InvalidModelException(string message) : base(message) { }
        private InvalidModelException(string message, Exception innerException) : base(message, innerException) { }
#pragma warning restore IDE0051 // Remove unused private members
        #endregion
    }
}
