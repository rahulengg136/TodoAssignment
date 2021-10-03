using AdFormAssignment.DAL.Entities;
using AdFormsAssignment.BLL.Contracts;
using AdFormsAssignment.BLL.Implementations;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AdFormsAssignment.Security
{
    public class JwtAuth : IJwtAuth
    {
        private readonly string _key;
        //private readonly IUserService _userService;
        public JwtAuth(string key 
            //IUserService userService
            )
        {
            _key = key;
            //_userService = userService;
        }
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
