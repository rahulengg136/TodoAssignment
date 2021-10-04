using AdFormAssignment.DAL.Contracts;
using AdFormAssignment.DAL.Entities;
using AdFormsAssignment.BLL.Contracts;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace AdFormsAssignment.BLL.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserDal _userDAL;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserDal userDAL, ILogger<UserService> logger)
        {
            _userDAL = userDAL;
            _logger = logger;
        }
        public async Task<TblUser> CheckUser(string username, string password)
        {
            return await _userDAL.CheckUser(username,password);
        }
    }
}
