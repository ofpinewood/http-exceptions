using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Opw.HttpExceptions
{
    /// <summary>
    /// Defines a serializable container for storing exception information.
    /// </summary>
    [Serializable]
    public class SerializableException : ISerializable
    {
        /// <summary>
        /// The type name of the exception.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets the exception instance that caused the current exception.
        /// </summary>
        public SerializableException InnerException { get; set; }

        /// <summary>
        /// Gets or sets a link to the help file associated with this exception.
        /// </summary>
        public string HelpLink { get; set; }

        /// <summary>
        /// Gets or sets HRESULT, a coded numerical value that is assigned to a specific exception.
        /// </summary>
        public int HResult { get; set; }

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the name of the application or the object that causes the error.
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Gets a string representation of the immediate frames on the call stack.
        /// </summary>
        public string StackTrace { get; set; }

        /// <summary>
        /// Gets a collection of key/value pairs that provide additional information about the exception.
        /// </summary>
        public IDictionary<string, object> Data { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Constructor used when Json deserializing.
        /// </summary>
        public SerializableException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableException"></see>.
        /// </summary>
        /// <param name="exception">The exception that needs serializing.</param>
        public SerializableException(Exception exception)
        {
            Type = exception.GetType().Name;

            if (exception.InnerException != null)
                InnerException = new SerializableException(exception.InnerException);

            HelpLink = exception.HelpLink;
            HResult = exception.HResult;
            Message = exception.Message;
            Source = exception.Source;
            StackTrace = exception.StackTrace;

            var propertiesToExclude = new string[] {
                nameof(Type),
                nameof(InnerException),
                nameof(HelpLink),
                nameof(HResult),
                nameof(Message),
                nameof(Source),
                nameof(StackTrace),
                nameof(exception.TargetSite)
            };

            foreach (var propertyInfo in exception.GetType().GetProperties())//.Where(p => p.GetCustomAttributes(typeof(ProblemDetailsAttribute), true)?.Any() != true))
            {
                if (propertiesToExclude.Any(p => p == propertyInfo.Name)) continue;
                if (propertyInfo.CanRead)
                    Data.Add(propertyInfo.Name, propertyInfo.GetValue(exception));
            }
        }

        /// <summary>
        /// Initializes a new instance of the exception class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"></see> that contains contextual information about the source or destination.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="info">info</paramref> parameter is null.</exception>
        /// <exception cref="SerializationException">The class name is null or <see cref="P:System.Exception.HResult"></see> is zero (0).</exception>
        public SerializableException(SerializationInfo info, StreamingContext context)
        {
            Type = info.GetValue(nameof(Type), typeof(string)) as string;
            InnerException = info.GetValue(nameof(InnerException), typeof(SerializableException)) as SerializableException;
            HelpLink = info.GetValue(nameof(HelpLink), typeof(string)) as string;

            var hResult = info.GetValue(nameof(HResult), typeof(int));
            if (hResult != null) HResult = (int)hResult;

            Message = info.GetValue(nameof(Message), typeof(string)) as string;
            Source = info.GetValue(nameof(Source), typeof(string)) as string;
            StackTrace = info.GetValue(nameof(StackTrace), typeof(string)) as string;
            Data = info.GetValue(nameof(Data), typeof(IDictionary<string, object>)) as IDictionary<string, object>;
        }

        /// <summary>
        /// Sets the <see cref="SerializationInfo"></see> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"></see> that contains contextual information about the source or destination.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="info">info</paramref> parameter is a null reference (Nothing in Visual Basic).</exception>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null) throw new ArgumentNullException(nameof(info));

            info.AddValue(nameof(Type), Type);
            info.AddValue(nameof(InnerException), InnerException);
            info.AddValue(nameof(HelpLink), HelpLink);
            info.AddValue(nameof(HResult), HResult);
            info.AddValue(nameof(Message), Message);
            info.AddValue(nameof(Source), Source);
            info.AddValue(nameof(StackTrace), StackTrace);
            info.AddValue(nameof(Data), Data);
        }
    }
}
