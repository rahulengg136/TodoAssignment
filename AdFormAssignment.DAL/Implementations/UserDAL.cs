using AdFormAssignment.DAL.Contracts;
using AdFormAssignment.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdFormAssignment.DAL.Implementations
{
    public class UserDAL : IUserDAL
    {
        private readonly MyProjectContext _dbContext;
        public UserDAL(MyProjectContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Task<tblUser> CheckUser(string username, string password)
        {
            var user = Task.FromResult(_dbContext.tblUser.SingleOrDefault(x => x.UserName == username && x.Password == password));
            return user;
        }
    }

}
