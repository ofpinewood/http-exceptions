using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Opw.HttpExceptions
{
    public class InvalidModelException : BadRequestException
    {
        public IEnumerable<ValidationResult> ValidationResults { get; protected set; }

        public InvalidModelException()
        { }

        public InvalidModelException(string errorMessage, string memberName, Exception innerException = null)
            : base("Invalid model.", innerException)
        {
            var validationResults = new List<ValidationResult>();
            validationResults.Add(new ValidationResult(errorMessage, new[] { memberName }));
            ValidationResults = validationResults;
        }

        public InvalidModelException(IEnumerable<ValidationResult> validationResults, Exception innerException = null)
            : base("Invalid model.", innerException)
        {
            ValidationResults = validationResults;
        }
    }
}
