using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Opw.HttpExceptions.AspNetCore._Test
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpPost("product")]
        public ActionResult<Product> PostProduct(Product product)
        {
            return Ok(product);
        }

        [Authorize]
        [HttpGet("authorized")]
        public ActionResult<Product> Authorized()
        {
            return Ok("Authorized");
        }
    }
}
