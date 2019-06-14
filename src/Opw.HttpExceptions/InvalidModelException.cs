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
        public IDictionary<string, string> ValidationResults { get; } = new Dictionary<string, string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidModelException"></see> class with status code BadRequest, a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message for the validation.</param>
        /// <param name="memberNames">The collection of member names that indicate which fields have validation.</param>
        public InvalidModelException(string message, params string[] memberNames) : this(message, null, memberNames) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidModelException"></see> class with status code BadRequest, a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message for the validation.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference
        /// if no inner exception is specified.</param>
        /// <param name="memberNames">The collection of member names that indicate which fields have validation.</param>
        public InvalidModelException(string message, Exception innerException, params string[] memberNames) : base(message, innerException)
        {
            foreach (var memberName in memberNames)
            {
                ValidationResults.Add(memberName, message);
            }
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
