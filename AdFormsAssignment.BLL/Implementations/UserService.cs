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
        public UserService(IUserDal userDAL)
        {
            _userDAL = userDAL;
        }
        public async Task<TblUser> CheckUser(string username, string password)
        {
            return await _userDAL.CheckUser(username, password);
        }
    }
}
