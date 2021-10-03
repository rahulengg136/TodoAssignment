using AdFormAssignment.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdFormsAssignment.BLL.Contracts
{
    public interface ILabelService
    {
        Task<int> CreateLabel(tblLabel label);
        Task<tblLabel> GetSingleLabelInfo(int labelId);
        Task<IEnumerable<tblLabel>> GetAllLabels(int PageNumber, int PageSize, string SearchText);
        Task<int> DeleteLabel(int labelId);
    }
}
