using AdFormAssignment.DAL.Entities;
using System.Threading.Tasks;

namespace AdFormsAssignment.BLL.Contracts
{
    /// <summary>
    /// Contract for user service
    /// </summary>
    public interface IUserService
    {
        Task<tblUser> CheckUser(string username, string password);
    }
}
