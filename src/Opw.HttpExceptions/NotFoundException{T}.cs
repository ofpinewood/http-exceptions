using System;
using System.Net;

namespace Opw.HttpExceptions
{
    public class NotFoundException<T> : HttpException
    {
        public NotFoundException() : base(HttpStatusCode.NotFound, "Not found.")
        {}

        public NotFoundException(string id)
            : base(HttpStatusCode.NotFound, $"{typeof(T).Name} \"{id}\" not found.")
        {
        }

        public NotFoundException(string id, Exception innerException)
            : base(HttpStatusCode.NotFound, $"{typeof(T).Name} \"{id}\" not found.", innerException)
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
