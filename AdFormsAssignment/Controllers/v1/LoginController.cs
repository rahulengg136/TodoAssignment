using AdFormsAssignment.DTO.PostDto;
using AdFormsAssignment.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        /// <returns>Returns token</returns>
        [AllowAnonymous]
        // POST api/<MembersController>
        [HttpPost("validateuser")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string),200)]
        [ProducesResponseType(typeof(string), 400)]
        public IActionResult ValidateUser([FromBody] UserDto userCredential)
        {
            var token = jwtAuth.Authentication(userCredential.Username, userCredential.Password);
            if (token == null)
                return BadRequest("Username and password is wrong");
            return Ok(token);
        }
    }
}
