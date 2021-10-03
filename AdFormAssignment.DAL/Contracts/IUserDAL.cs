using AdFormAssignment.DAL.Entities;
using System.Threading.Tasks;

namespace AdFormAssignment.DAL.Contracts
{
    public interface IUserDAL
    {
        Task<tblUser> CheckUser(string username, string password);
    }
}
