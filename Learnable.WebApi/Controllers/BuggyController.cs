using Microsoft.AspNetCore.Mvc;

namespace Learnable.WebApi.Controllers
{
    public class BuggyController : BaseController
    {
        [HttpGet("auth")] // http://localhost:5126/api/buggy/auth
        public IActionResult GetPerson()
        {
            return Unauthorized();
        }

        [HttpGet("not-found")] // http://localhost:5126/api/buggy/not-found
        public IActionResult GetNotFound()
        {
            return NotFound();
        }

        [HttpGet("server-error")] // http://localhost:5126/api/buggy/server-error
        public IActionResult GetServerError()
        {
            throw new Exception("This is a server error");
        }

        [HttpGet("bad-request")] // http://localhost:5126/api/buggy/bad-request
        public IActionResult GetBadRequest()
        {
            return BadRequest("This is a bad request");
        }
    }
}
