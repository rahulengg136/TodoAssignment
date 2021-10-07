using AdFormAssignment.DAL.Contracts;
using AdFormAssignment.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdFormAssignment.DAL.Implementations
{
    public class LabelDal : ILabelDal
    {
        private readonly MyProjectContext _dbContext;
        public LabelDal(MyProjectContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CreateLabel(TblLabel label)
        {
            Log.Information("Going to hit database");
            await _dbContext.TblLabel.AddAsync(label);
            await _dbContext.SaveChangesAsync();
            return label.LabelId;
        }

        public async Task<int> DeleteLabel(int labelId)
        {
            Log.Information("Going to hit database");
            var labelToDelete = await _dbContext.TblLabel.SingleOrDefaultAsync(x => x.LabelId == labelId);
            _dbContext.TblLabel.Remove(labelToDelete);
            await _dbContext.SaveChangesAsync();
            return labelId;
        }

        public Task<IEnumerable<TblLabel>> GetAllLabels(int pageNumber, int pageSize, string searchText)
        {
            Log.Information("Going to hit database");
            return Task.FromResult(_dbContext.TblLabel.Where(x => ((searchText == null) || x.LabelName.Contains(searchText))).Skip((pageNumber - 1) * pageSize).Take(pageSize).AsEnumerable());
        }

        public async Task<TblLabel> GetSingleLabel(int labelId)
        {
            Log.Information("Going to hit database");
            return await _dbContext.TblLabel.SingleOrDefaultAsync(x => x.LabelId == labelId);
        }
    }
}
