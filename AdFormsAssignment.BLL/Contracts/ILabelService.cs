using AdFormAssignment.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdFormsAssignment.BLL.Contracts
{
    /// <summary>
    /// Contract for label service
    /// </summary>
    public interface ILabelService
    {
        Task<int> CreateLabel(TblLabel label);
        Task<TblLabel> GetSingleLabelInfo(int labelId);
        Task<IEnumerable<TblLabel>> GetAllLabels(int pageNumber, int pageSize, string searchText);
        Task<int> DeleteLabel(int labelId);
    }
}
