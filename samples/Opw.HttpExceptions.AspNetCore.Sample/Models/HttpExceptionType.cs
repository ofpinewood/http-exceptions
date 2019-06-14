namespace Opw.HttpExceptions.AspNetCore.Sample.Models
{
    public enum HttpExceptionType
    {
        BadRequest = 400,
        NotFound = 404,
        NotFoundT = 1404,
        InternalServerError = 500,
        Forbidden = 403,
        Unauthorized = 401
    }
}
