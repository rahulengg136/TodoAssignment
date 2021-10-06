using AdFormAssignment.DAL.Contracts;
using AdFormAssignment.DAL.Entities;
using AdFormsAssignment.BLL.Contracts;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdFormsAssignment.BLL.Implementations
{
    public class LabelService : ILabelService
    {
        private readonly ILabelDal _labelDAL;

        public LabelService(ILabelDal labelDAL)
        {
            _labelDAL = labelDAL;
        }
        public async Task<int> CreateLabel(TblLabel label)
        {
            Log.Information($"Going to hit DAL method");
            return await _labelDAL.CreateLabel(label);
        }
        public async Task<int> DeleteLabel(int labelId)
        {
            Log.Information($"Going to hit DAL method");
            return await _labelDAL.DeleteLabel(labelId);
        }
        public async Task<IEnumerable<TblLabel>> GetAllLabels(int PageNumber, int PageSize, string SearchText)
        {
            Log.Information($"Going to hit DAL method");
            PageNumber = PageNumber == 0 ? 1 : PageNumber;
            PageSize = PageSize == 0 ? int.MaxValue : PageSize;
            return await _labelDAL.GetAllLabels(PageNumber, PageSize, SearchText);
        }

        public async Task<TblLabel> GetSingleLabelInfo(int labelId)
        {
            Log.Information($"Going to hit DAL method");
            var label = await _labelDAL.GetSingleLabel(labelId);
            return label;
        }
    }
}
