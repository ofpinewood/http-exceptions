using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System;

namespace Opw.HttpExceptions.AspNetCore.Sample.CustomErrors
{
    public class CustomErrorResult : ObjectResult
    {
        public new CustomError Value
        {
            get => (CustomError)base.Value;
            set => base.Value = value;
        }

        public CustomErrorResult(CustomError customError) : base(customError)
        {
            _ = customError ?? throw new ArgumentNullException(nameof(customError));

            StatusCode = customError.Status;
            DeclaredType = customError.GetType();

            ContentTypes.Add(new MediaTypeHeaderValue("application/problem+json"));
            ContentTypes.Add(new MediaTypeHeaderValue("application/problem+xml"));
        }
    }
}
