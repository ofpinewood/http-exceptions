using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Opw.HttpExceptions
{
    /// <summary>
    /// Represents HTTP BadRequest (400) errors because of invalid input.
    /// </summary>
    [Serializable]
    public class InvalidModelException : ValidationErrorException<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidModelException"></see> class with status code BadRequest.
        /// </summary>
        /// <param name="errors">The error messages for the member.</param>
        /// <param name="memberName">The member name that indicate which field have an error.</param>
        public InvalidModelException(string memberName, params string[] errors) : base(memberName, errors) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidModelException"></see> class with status code BadRequest.
        /// </summary>
        /// <param name="errors"> The validation errors.</param>
        public InvalidModelException(IDictionary<string, string[]> errors) : base(errors) { }

        /// <summary>
        /// Initializes a new instance of the exception class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"></see> that contains contextual information about the source or destination.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="info">info</paramref> parameter is null.</exception>
        /// <exception cref="SerializationException">The class name is null or <see cref="P:System.Exception.HResult"></see> is zero (0).</exception>
        public InvalidModelException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
