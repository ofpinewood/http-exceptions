using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json;

namespace Opw.HttpExceptions.AspNetCore
{
    /// <summary>
    /// Provides extension methods for ProblemDetails.
    /// </summary>
    public static class ProblemDetailsExtensions
    {
        /// <summary>
        /// Try to get the exception details from the ProblemDetails.
        /// </summary>
        /// <param name="problemDetails">ProblemDetails to get the exception from.</param>
        /// <param name="exception">When this method returns, the exception from the ProblemDetails, if the ProblemDetails contains an exception; otherwise, null.</param>
        /// <returns>true if the ProblemDetails contains an exception; otherwise, false.</returns>
        public static bool TryGetExceptionDetails(this ProblemDetails problemDetails, out SerializableException exception)
        {
            if (problemDetails.Extensions.TryGetValue(nameof(ProblemDetailsExtensionMembers.ExceptionDetails).ToCamelCase(), out var value))
                return value.TryParseSerializableException(out exception);

            exception = null;
            return false;
        }

        /// <summary>
        /// Try to parse a SerializableException from an object.
        /// </summary>
        /// <param name="value">The object to parse the SerializableException from.</param>
        /// <param name="exception">When this method returns, the exception from the object, if the object can be parsed to an SerializableException; otherwise, null.</param>
        /// <returns>true if the object can be parsed to an SerializableException; otherwise, false.</returns>
        public static bool TryParseSerializableException(this object value, out SerializableException exception)
        {
            exception = null;

            if (value is SerializableException serializableException)
                exception = serializableException;

            if (value is JsonElement jsonElement)
            {
                var str = jsonElement.GetRawText();
                exception = JsonSerializer.Deserialize<SerializableException>(str, new JsonSerializerOptions());
            }

            return exception != null;
        }

        /// <summary>
        /// Try to get the errors dictionary from the ProblemDetails.
        /// </summary>
        /// <param name="problemDetails">ProblemDetails to get the errors dictionary from.</param>
        /// <param name="errors">When this method returns, the errors dictionary from the ProblemDetails, if the ProblemDetails contains errors dictionary; otherwise, null.</param>
        /// <returns>true if the ProblemDetails contains errors dictionary; otherwise, false.</returns>
        public static bool TryGetErrors(this ProblemDetails problemDetails, out IDictionary<string, object[]> errors)
        {
            if (problemDetails.Extensions.TryGetValue(nameof(ProblemDetailsExtensionMembers.Errors).ToCamelCase(), out var value))
                return value.TryParseErrors(out errors);

            errors = null;
            return false;
        }

        /// <summary>
        /// Try to parse a errors dictionary from an object.
        /// </summary>
        /// <param name="value">The object to parse the errors dictionary from.</param>
        /// <param name="errors">When this method returns, the errors dictionary from the object, if the object can be parsed to an errors dictionary; otherwise, null.</param>
        /// <returns>true if the object can be parsed to an errors dictionary; otherwise, false.</returns>
        public static bool TryParseErrors(this object value, out IDictionary<string, object[]> errors)
        {
            errors = null;

#pragma warning disable RCS1220 // Use pattern matching instead of combination of 'is' operator and cast operator.
            if (value is IDictionary<string, object[]>)
                errors = (IDictionary<string, object[]>)value;
#pragma warning restore RCS1220 // Use pattern matching instead of combination of 'is' operator and cast operator.

            return errors != null;
        }
    }
}
