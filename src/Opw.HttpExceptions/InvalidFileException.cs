using System;

namespace Opw.HttpExceptions
{
    public class InvalidFileException : BadRequestException
    {
        public InvalidFileException()
        {
        }

        public InvalidFileException(string message) : base(message)
        {
        }

        public InvalidFileException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
