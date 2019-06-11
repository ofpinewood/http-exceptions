using System;
using System.Net;

namespace Opw.HttpExceptions
{
    public class SerializationErrorException<T> : HttpException
    {
        public SerializationErrorException() : base(HttpStatusCode.InternalServerError, $"Error during serialization or deserialization of {typeof(T).Name}.")
        {}

        public SerializationErrorException(string id)
            : base(HttpStatusCode.InternalServerError, $"Error during serialization or deserialization of {typeof(T).Name} \"{id}\".")
        {
        }

        public SerializationErrorException(string id, Exception innerException)
            : base(HttpStatusCode.InternalServerError, $"Error during serialization or deserialization of {typeof(T).Name} \"{id}\".", innerException)
        {
        }

        protected override string GetTitle(string typeName = null)
        {
            typeName = typeof(T).Name;
            if (typeof(T).IsInterface)
                typeName = typeName.Substring(1);

            return base.GetTitle(typeName);
        }
    }
}
