using AdFormAssignment.DAL.Contracts;
using AdFormAssignment.DAL.Entities;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdFormAssignment.DAL.Implementations
{
    public class LabelDAL : ILabelDAL
    {
        private readonly MyProjectContext _dbContext;
        public LabelDAL(MyProjectContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CreateLabel(tblLabel labelInfo)
        {
            Log.Information($"Going to hit database");
            _dbContext.tblLabel.Add(labelInfo);
            await _dbContext.SaveChangesAsync();
            return labelInfo.LabelId;
        }

        public async Task<int> DeleteLabel(int labelId)
        {
            Log.Information($"Going to hit database");
            var labelToDelete = _dbContext.tblLabel.Single(x => x.LabelId == labelId);
            _dbContext.tblLabel.Remove(labelToDelete);
            await _dbContext.SaveChangesAsync();
            return labelId;
        }

        public Task<IEnumerable<tblLabel>> GetAllLabels(int PageNumber, int PageSize, string SearchText)
        {
            Log.Information($"Going to hit database");
            throw new Exception();
            return Task.FromResult(_dbContext.tblLabel.Where(x => SearchText != null ? x.LabelName.Contains(SearchText) : true).Skip((PageNumber - 1) * PageSize).Take(PageSize).AsEnumerable());

        }

        public Task<tblLabel> GetSingleLabel(int labelId)
        {
            Log.Information($"Going to hit database");
            return Task.FromResult(_dbContext.tblLabel.SingleOrDefault(x => x.LabelId == labelId));
        }
    }
}
