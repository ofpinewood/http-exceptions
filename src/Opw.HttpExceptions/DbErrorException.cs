using System;

namespace Opw.HttpExceptions
{
    public class DbErrorException : InternalServerErrorException
    {
        public DbErrorException()
        {
        }

        public DbErrorException(string message) : base(message)
        {
        }

        public DbErrorException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
