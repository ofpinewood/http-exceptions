using Microsoft.AspNetCore.Mvc;
using Opw.HttpExceptions.AspNetCore.Sample.Models;
using System;
using System.Net;

namespace Opw.HttpExceptions.AspNetCore.Sample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HttpExceptionController : ControllerBase
    {
        [HttpGet("{type}")]
        public ActionResult<string> Get(HttpExceptionType type)
        {
            var message = $"{type}Error has occurred.";
            var innerException = new ApplicationException($"Inner exception for {type}Error.");
            switch (type)
            {
                case HttpExceptionType.BadRequest:
                    throw new BadRequestException(message, innerException);
                case HttpExceptionType.Forbidden:
                    throw new ForbiddenException(message, innerException);
                case HttpExceptionType.NotFound:
                    throw new NotFoundException(message, innerException);
                case HttpExceptionType.NotFoundT:
                    throw new NotFoundException<Customer>(message, innerException);
                case HttpExceptionType.Unauthorized:
                    throw new UnauthorizedException(message, innerException);
                default:
                    throw new HttpException((HttpStatusCode)((int)type), message, innerException);
            }
        }
    }
}
