using AdFormAssignment.DAL.Entities;
using System.Threading.Tasks;

namespace AdFormAssignment.DAL.Contracts
{
    /// <summary>
    /// Contracts for user data access
    /// </summary>
    public interface IUserDal
    {
        Task<TblUser> CheckUser(string username, string password);
    }
}
