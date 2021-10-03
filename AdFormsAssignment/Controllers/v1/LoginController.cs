using AdFormsAssignment.DTO;
using AdFormsAssignment.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdFormsAssignment.Controllers
{
    /// <summary>
    /// This controller performs taks related to login and Security
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IJwtAuth jwtAuth;
        /// <summary>
        /// Login controller 
        /// </summary>
        /// <param name="jwtAuth"></param>
        public LoginController(IJwtAuth jwtAuth)
        {
            this.jwtAuth = jwtAuth;
        }
        /// <summary>
        /// This method validates user and provides token for 1 hour
        /// </summary>
        /// <param name="userCredential">Need to pass username and password in body</param>
        /// <returns></returns>
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
