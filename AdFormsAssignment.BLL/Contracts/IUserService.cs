using AdFormAssignment.DAL.Entities;
using System.Threading.Tasks;

namespace AdFormsAssignment.BLL.Contracts
{
    public interface IUserService
    {
        Task<tblUser> CheckUser(string username, string password);
    }
}
