using System;

namespace Opw.HttpExceptions.AspNetCore.Mappers
{
    /// <summary>
    /// Represents a HttpResponseMapperDescriptor.
    /// </summary>
    public class HttpResponseMapperDescriptor
    {
        /// <summary>
        /// The type of the described HttpResponseMapper;
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// Optionally parameters to inject through the HttpResponseMapper constructor.
        /// </summary>
        public object[] Arguments { get; set; }
    }
}
