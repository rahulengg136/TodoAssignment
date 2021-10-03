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
        private readonly ILabelDAL _labelDAL;

        public LabelService(ILabelDAL labelDAL)
        {
            _labelDAL = labelDAL;
        }
        public async Task<int> CreateLabel(tblLabel label)
        {
            Log.Information($"Going to hit DAL method");
            return await _labelDAL.CreateLabel(label);
        }

        public async Task<int> DeleteLabel(int labelId)
        {
            Log.Information($"Going to hit DAL method");
            return await _labelDAL.DeleteLabel(labelId);
        }

        public async Task<IEnumerable<tblLabel>> GetAllLabels(int PageNumber, int PageSize, string SearchText)
        {

            Log.Information($"Going to hit DAL method");
            return await _labelDAL.GetAllLabels(PageNumber, PageSize, SearchText);
        }

        public async Task<tblLabel> GetSingleLabelInfo(int labelId)
        {
            Log.Information($"Going to hit DAL method");
            return await _labelDAL.GetSingleLabel(labelId);
        }
    }
}
