using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Opw.HttpExceptions
{
    /// <summary>
    /// A helper class for serializing exceptions.
    /// </summary>
    public class ExceptionInfo : Dictionary<string, object>
    {
        /// <summary>
        /// The type name of the exception.
        /// </summary>
        public string Type => GetValue<string>(nameof(Type));

        /// <summary>
        /// Gets a collection of key/value pairs that provide additional user-defined information about the exception.
        /// </summary>
        public IDictionary Data => GetValue<IDictionary>(nameof(Data));

        /// <summary>
        /// Gets the exception instance that caused the current exception.
        /// </summary>
        public object InnerException => GetValue<object>(nameof(InnerException));

        /// <summary>
        /// Gets or sets a link to the help file associated with this exception.
        /// </summary>
        public string HelpLink => GetValue<string>(nameof(HelpLink));

        /// <summary>
        /// Gets or sets HRESULT, a coded numerical value that is assigned to a specific exception.
        /// </summary>
        public int HResult => GetValue<int>(nameof(HResult));

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        public string Message => GetValue<string>(nameof(Message));

        /// <summary>
        /// Gets or sets the name of the application or the object that causes the error.
        /// </summary>
        public string Source => GetValue<string>(nameof(Source));

        /// <summary>
        /// Gets a string representation of the immediate frames on the call stack.
        /// </summary>
        public string StackTrace => GetValue<string>(nameof(StackTrace));

        /// <summary>
        /// Constructor used when Json deserializing.
        /// </summary>
        public ExceptionInfo() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionInfo"></see>.
        /// </summary>
        /// <param name="exception">The exception that needs serializing.</param>
        public ExceptionInfo(Exception exception)
        {
            Add(nameof(Type), exception.GetType().Name);

            if (exception.InnerException != null)
                Add(nameof(InnerException), new ExceptionInfo(exception.InnerException));

            var propertiesToExclude = new string[] {
                Type,
                nameof(InnerException),
                nameof(exception.TargetSite)
            };

            foreach (var propertyInfo in exception.GetType().GetProperties())
            {
                if (propertiesToExclude.Any(p => p == propertyInfo.Name)) continue;
                if (propertyInfo.CanRead)
                    Add(propertyInfo.Name, propertyInfo.GetValue(exception));
            }
        }

        private T GetValue<T>(string key)
        {
            if (TryGetValue(key, out var value))
                return (T)value;
            return default;
        }
    }
}
