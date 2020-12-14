using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;

namespace Opw.HttpExceptions.AspNetCore
{
    /// <summary>
    /// A ProblemDetailsResult implementation of Microsoft.AspNetCore.Mvc.ObjectResult.
    /// </summary>
    public class ProblemDetailsResult : ObjectResult
    {
        /// <summary>
        /// ProblemDetails.
        /// </summary>
        public new ProblemDetails Value
        {
            get => (ProblemDetails)base.Value;
            set => base.Value = value;
        }

        /// <summary>
        /// Initializes the ProblemDetailsResult.
        /// </summary>
        /// <param name="problemDetails">The ProblemDetails</param>
        public ProblemDetailsResult(ProblemDetails problemDetails) : base(problemDetails)
        {
            _ = problemDetails ?? throw new ArgumentNullException(nameof(problemDetails));

            StatusCode = problemDetails.Status;
            DeclaredType = problemDetails.GetType();

            ContentTypes.Add(new MediaTypeHeaderValue("application/problem+json"));
            ContentTypes.Add(new MediaTypeHeaderValue("application/problem+xml"));
        }
    }
}
