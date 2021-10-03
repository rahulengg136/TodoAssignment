using AdFormAssignment.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdFormsAssignment.BLL.Contracts
{
   public interface IUserService
    {
        Task<tblUser> CheckUser(string username, string password);
    }
}
