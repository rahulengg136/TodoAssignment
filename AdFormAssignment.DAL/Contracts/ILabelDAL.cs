using AdFormAssignment.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdFormAssignment.DAL.Contracts
{
    /// <summary>
    /// Contract for labels data access
    /// </summary>
    public interface ILabelDal
    {
        Task<int> CreateLabel(TblLabel label);
        Task<TblLabel> GetSingleLabel(int labelId);
        Task<IEnumerable<TblLabel>> GetAllLabels(int pageNumber, int pageSize, string searchText);
        Task<int> DeleteLabel(int labelId);
    }
}
