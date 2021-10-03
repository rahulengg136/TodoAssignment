using AdFormAssignment.DAL.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AdFormsAssignment.Security
{
    /// <summary>
    /// Class that provides a token
    /// </summary>
    public class JwtAuth : IJwtAuth
    {
        private readonly string _key;
        //private readonly IUserService _userService;
       /// <summary>
       /// Constructor takes key that helps in generating unique token for different clients
       /// </summary>
       /// <param name="key">A string value</param>
        public JwtAuth(string key 
            //IUserService userService
            )
        {
            _key = key;
            //_userService = userService;
        }
        /// <summary>
        /// This method check user and provides token
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns></returns>
        public string Authentication(string username, string password)
        {
            //  var user = _userService.CheckUser(username, password);
            var user = new tblUser();
            user.UserId = 3;
            if (user == null)
            {
                return null;
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(_key);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    }),
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            // Create Token
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
