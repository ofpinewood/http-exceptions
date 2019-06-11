using System;
using System.Net;

namespace Opw.HttpExceptions
{
    public abstract class HttpException : Exception
    {
        /// <summary>
        /// A short, human-readable summary of the problem type. It SHOULD NOT change from occurrence to occurrence of the
        /// problem, except for purposes of localization (e.g., using proactive content negotiation; see[RFC7231], Section 3.4).
        /// </summary>
        public string Title
        {
            get { return GetTitle(); }
        }

        public int StatusCode { get; protected set; }

        public HttpException(HttpStatusCode statusCode, string message)
			: base(message)
		{
            StatusCode = (int)statusCode;
        }

        public HttpException(int statusCode, string message)
            : base(message)
        {
            StatusCode = statusCode;
        }

        public HttpException(HttpStatusCode statusCode, string message, Exception innerException)
            : base(message, innerException)
        {
            StatusCode = (int)statusCode;
        }

        public HttpException(int statusCode, string message, Exception innerException)
			: base(message, innerException)
		{
            StatusCode = statusCode;
        }

        protected virtual string GetTitle(string typeName = null)
        {
            var name = GetType().Name;
            if (name.Contains("`")) name = name.Substring(0, name.IndexOf('`'));
            if (name.EndsWith("Exception", StringComparison.InvariantCultureIgnoreCase))
                name = name.Substring(0, name.Length - "Exception".Length);

            if (typeName == null) return name;
            return $"{typeName}{name}";
        }
    }
}
