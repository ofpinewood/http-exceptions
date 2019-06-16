using System;

namespace Opw.HttpExceptions.AspNetCore
{
    /// <summary>
    /// Represents a ExceptionMapperDescriptor.
    /// </summary>
    public class ExceptionMapperDescriptor
    {
        /// <summary>
        /// The type of the described ExceptionMapper;
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// Optionally parameters to inject through the ExceptionMapper constructor.
        /// </summary>
        public object[] Arguments { get; set; }
    }
}
