using AdFormsAssignment.DTO;
using AdFormsAssignment.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdFormsAssignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IJwtAuth jwtAuth;
        public LoginController(IJwtAuth jwtAuth)
        {
            this.jwtAuth = jwtAuth;
        }
        [AllowAnonymous]
        // POST api/<MembersController>
        [HttpPost("validateuser")]
        public IActionResult ValidateUser([FromBody] UserDto userCredential)
        {
            var token = jwtAuth.Authentication(userCredential.UserName, userCredential.Password);
            if (token == null)
                return Unauthorized();
            return Ok(token);
        }
    }
}
