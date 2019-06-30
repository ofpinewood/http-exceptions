using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Opw.HttpExceptions.AspNetCore.Sample.Models;
using System;
using System.Net;

namespace Opw.HttpExceptions.AspNetCore.Sample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet("{statusCode}")]
        public ActionResult<string> Throw(HttpStatusCode statusCode)
        {
            var message = $"{statusCode}Error has occurred.";
            var innerException = new ApplicationException($"Inner exception for {statusCode}Error.");
            throw new HttpException(statusCode, message, innerException);
        }

        [HttpGet("applicationException")]
        public ActionResult<string> ThrowApplicationException()
        {
            var innerException = new ApplicationException($"Inner exception for ApplicationException.");
            throw new ApplicationException("ApplicationException has occurred.", innerException);
        }

        [HttpPost("product")]
        public ActionResult<Product> PostProduct(Product product)
        {
            return Ok(product);
        }

        [Authorize]
        [HttpGet("authorized")]
        public ActionResult<string> Authorized()
        {
            return Ok("Authorized");
        }

        [HttpGet("customError")]
        public ActionResult<Product> ReturnCustomError()
        {
            throw new FormatException("Custom Error!");
        }
    }
}
