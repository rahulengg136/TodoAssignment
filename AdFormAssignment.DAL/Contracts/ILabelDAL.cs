using AdFormAssignment.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdFormAssignment.DAL.Contracts
{
    /// <summary>
    /// Contract for labels data access
    /// </summary>
    public interface ILabelDAL
    {
        Task<int> CreateLabel(tblLabel label);
        Task<tblLabel> GetSingleLabel(int labelId);
        Task<IEnumerable<tblLabel>> GetAllLabels(int PageNumber, int PageSize, string SearchText);
        Task<int> DeleteLabel(int labelId);
    }
}
