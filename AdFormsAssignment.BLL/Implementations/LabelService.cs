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
        private readonly ILabelDal _labelDal;

        public LabelService(ILabelDal labelDal)
        {
            _labelDal = labelDal;
        }
        public async Task<int> CreateLabel(TblLabel label)
        {
            Log.Information("Going to hit DAL method");
            return await _labelDal.CreateLabel(label);
        }
        public async Task<int> DeleteLabel(int labelId)
        {
            Log.Information("Going to hit DAL method");
            return await _labelDal.DeleteLabel(labelId);
        }
        public async Task<IEnumerable<TblLabel>> GetAllLabels(int pageNumber, int pageSize, string searchText)
        {
            Log.Information("Going to hit DAL method");
            pageNumber = pageNumber == 0 ? 1 : pageNumber;
            pageSize = pageSize == 0 ? int.MaxValue : pageSize;
            return await _labelDal.GetAllLabels(pageNumber, pageSize, searchText);
        }

        public async Task<TblLabel> GetSingleLabelInfo(int labelId)
        {
            Log.Information("Going to hit DAL method");
            var label = await _labelDal.GetSingleLabel(labelId);
            return label;
        }
    }
}
