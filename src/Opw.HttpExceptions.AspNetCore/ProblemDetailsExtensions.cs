using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json;

namespace Opw.HttpExceptions.AspNetCore
{
    public static class ProblemDetailsExtensions
    {
        public static bool TryGetExceptionDetails(this ProblemDetails problemDetails, out SerializableException exception)
        {
            if (problemDetails.Extensions.TryGetValue(nameof(ProblemDetailsExtensionMembers.ExceptionDetails).ToCamelCase(), out var value))
                return value.TryParseSerializableException(out exception);

            exception = null;
            return false;
        }

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

        public static bool TryGetErrors(this ProblemDetails problemDetails, out IDictionary<string, object[]> errors)
        {
            if (problemDetails.Extensions.TryGetValue(nameof(ProblemDetailsExtensionMembers.Errors).ToCamelCase(), out var value))
                return value.TryParseErrors(out errors);

            errors = null;
            return false;
        }

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
