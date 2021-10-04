using AdFormAssignment.DAL.Contracts;
using AdFormAssignment.DAL.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace AdFormAssignment.DAL.Implementations
{
    public class UserDal : IUserDal
    {
        private readonly MyProjectContext _dbContext;
        public UserDal(MyProjectContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Task<TblUser> CheckUser(string username, string password)
        {
            var user = Task.FromResult(_dbContext.tblUser.SingleOrDefault(x => x.UserName == username && x.Password == password));
            return user;
        }
    }

}
