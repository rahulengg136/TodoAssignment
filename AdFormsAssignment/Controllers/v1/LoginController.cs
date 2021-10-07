using AdFormsAssignment.DTO.Common;
using AdFormsAssignment.DTO.PostDto;
using AdFormsAssignment.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdFormsAssignment.Controllers.v1
{
    /// <summary>
    /// This controller performs tasks related to login and Security
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IJwtAuth _jwtAuth;
        /// <summary>
        /// Login controller 
        /// </summary>
        /// <param name="jwtAuth"></param>
        public LoginController(IJwtAuth jwtAuth)
        {
            this._jwtAuth = jwtAuth;
        }
        /// <summary>
        /// This method validates user and provides token for 1 hour
        /// </summary>
        /// <param name="userCredential">Need to pass username and password in body</param>
        /// <returns>Returns token</returns>
        [AllowAnonymous]
        // POST api/<MembersController>
        [HttpPost("validate")]
        [ProducesResponseType(typeof(ValidatedUserToken), 200)]
        [ProducesResponseType(typeof(BadRequestInfo), 400)]
        public IActionResult ValidateUser([FromBody] UserDto userCredential)
        {
            var token = _jwtAuth.Authentication(userCredential.Username, userCredential.Password);
            if (token == null)
                return BadRequest(new BadRequestInfo() { Message = "Username and password is wrong" });
            return Ok(new { Message = token });
        }
    }
}
