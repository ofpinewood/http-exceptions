using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Opw.HttpExceptions
{
    /// <summary>
    /// Represents HTTP BadRequest (400) errors because of an invalid model.
    /// </summary>
    [Serializable]
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

        /// <summary>
        /// Initializes a new instance of the exception class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"></see> that contains contextual information about the source or destination.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="info">info</paramref> parameter is null.</exception>
        /// <exception cref="SerializationException">The class name is null or <see cref="P:System.Exception.HResult"></see> is zero (0).</exception>
        public InvalidModelException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Errors = info.GetValue(nameof(Errors), typeof(IDictionary<string, string[]>)) as IDictionary<string, string[]>;
        }

        #region private constructors to disable warning RCS1194 Implement exception constructors
#pragma warning disable IDE0051 // Remove unused private members
        private InvalidModelException() : base() { }
        private InvalidModelException(string message) : base(message) { }
        private InvalidModelException(string message, Exception innerException) : base(message, innerException) { }
#pragma warning restore IDE0051 // Remove unused private members
        #endregion

        /// <summary>
        /// Sets the <see cref="SerializationInfo"></see> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"></see> that contains contextual information about the source or destination.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="info">info</paramref> parameter is a null reference (Nothing in Visual Basic).</exception>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null) throw new ArgumentNullException(nameof(info));

            info.AddValue(nameof(Errors), Errors);
            base.GetObjectData(info, context);
        }
    }
}
