using AdFormAssignment.DAL.Entities;
using System.Threading.Tasks;

namespace AdFormAssignment.DAL.Contracts
{
    /// <summary>
    /// Contracts for user data access
    /// </summary>
    public interface IUserDAL
    {
        Task<tblUser> CheckUser(string username, string password);
    }
}
