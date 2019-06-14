using System;

namespace Opw.HttpExceptions.AspNetCore
{
    /// <summary>
    /// Represents an error in a serializable form.
    /// </summary>
    public class ExceptionDetails
    {
        /// <summary>
        /// Gets the name of the current exception.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets a string representation of the immediate frames on the call stack.
        /// </summary>
        public string StackTrace { get; set; }

        /// <summary>
        /// Gets or sets the name of the application or the object that causes the error.
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets the ExceptionDetails for the exception that caused the current exception.
        /// </summary>
        public ExceptionDetails InnerException { get; }

        /// <summary>
        /// Initializes the ExceptionDetails.
        /// </summary>
        /// <param name="ex">The exception to use.</param>
        public ExceptionDetails(Exception ex)
        {
            Name = ex.GetType().Name;
            StackTrace = ex.StackTrace;
            Source = ex.Source;
            Message = ex.Message;

            if (ex.InnerException != null)
                InnerException = new ExceptionDetails(ex.InnerException);
        }
    }
}
