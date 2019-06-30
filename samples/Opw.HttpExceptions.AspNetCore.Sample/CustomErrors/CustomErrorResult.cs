using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

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
            StatusCode = customError.Status;
            DeclaredType = customError.GetType();

            ContentTypes.Add(new MediaTypeHeaderValue("application/problem+json"));
            ContentTypes.Add(new MediaTypeHeaderValue("application/problem+xml"));
        }
    }
}
