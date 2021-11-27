using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetBook4.Filters;

namespace TweetBook4.Controllers.v1
{
    [ApiKeyAuth]
    [Route("api/[controller]")]
    [ApiController]
    public class SecretController : ControllerBase
    {
        [HttpGet("secret")]
        public IActionResult GetSecret()
        {
            return Ok("I have no Secret");
        }
    }
}
