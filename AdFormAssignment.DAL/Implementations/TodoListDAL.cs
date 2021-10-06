using AdFormAssignment.DAL.Contracts;
using AdFormAssignment.DAL.Entities;
using AdFormsAssignment.DTO;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace AdFormAssignment.DAL
{
    public class TodoListDal : ITodoListDal
    {
        private readonly MyProjectContext _dbContext;
        public TodoListDal(MyProjectContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<TodoListDetail> GetTodoList(int todoListId, int userId)
        {
            var data = _dbContext.TblTodoList.SingleOrDefault(x => x.TodoListId == todoListId);
            if (data != null)
            {
                TodoListDetail todoListDetail = JsonSerializer.Deserialize<TodoListDetail>(JsonSerializer.Serialize(data));
                int[] recordRelevantLabelsIds = _dbContext.TblLabelMapping.Where(x => x.RecordId == todoListId && x.TodoTypeId == 1).Select(x => x.LabelId).ToArray();
                todoListDetail.Labels = _dbContext.TblLabel.Where(x => recordRelevantLabelsIds.Contains(x.LabelId)).ToList();
                return Task.FromResult(todoListDetail);
            }
            else
            {
                return Task.FromResult<TodoListDetail>(null);
            }
        }

        public Task<IEnumerable<TodoListDetail>> GetAllTodoLists(int PageNumber, int PageSize, string SearchText, int userId)
        {
            Log.Information("Going to hit database");
            var data = _dbContext.TblTodoList.Where(x => ((SearchText == null) || x.ListName.Contains(SearchText)) && x.UserId == userId).Skip((PageNumber - 1) * PageSize).Take(PageSize).AsEnumerable();

            IEnumerable<TodoListDetail> todoListsDetail = JsonSerializer.Deserialize<IEnumerable<TodoListDetail>>(JsonSerializer.Serialize(data));

            int[] allToDoListsId = data.Select(x => x.TodoListId).ToArray();

            List<TblLabelMapping> mappingsOfAllLists = _dbContext.TblLabelMapping.Where(x => allToDoListsId.Contains(x.RecordId) && x.TodoTypeId == 1).ToList();
            int[] allRequiredLabelIds = _dbContext.TblLabelMapping.Where(x => allToDoListsId.Contains(x.RecordId) && x.TodoTypeId == 1).Select(x => x.LabelId).ToArray();
            List<TblLabel> allLabelsRequired = _dbContext.TblLabel.Where(x => allRequiredLabelIds.Contains(x.LabelId)).ToList();

            foreach (var listDetail in todoListsDetail)
            {
                int[] labelIdsOfCurrentList = mappingsOfAllLists.Where(x => x.RecordId == listDetail.TodoListId).Select(x => x.LabelId).ToArray();
                listDetail.Labels = allLabelsRequired.Where(x => labelIdsOfCurrentList.Contains(x.LabelId)).ToList();
            }
            return Task.FromResult(todoListsDetail);
        }

        public async Task<int> CreateTodoList(TblTodoList todoList, IEnumerable<TblLabelMapping> mappings)
        {
            Log.Information("Going to hit database");
            _dbContext.TblTodoList.Add(todoList);
            await _dbContext.SaveChangesAsync();

            mappings.ToList().ForEach(x => x.RecordId = todoList.TodoListId);
            _dbContext.TblLabelMapping.AddRange(mappings);
            await _dbContext.SaveChangesAsync();

            return todoList.TodoListId;
        }
        public async Task<int> DeleteTodoList(int todoListId)
        {
            Log.Information("Going to hit database");
            var listToDelete = _dbContext.TblTodoList.Single(x => x.TodoListId == todoListId);
            _dbContext.TblTodoList.Remove(listToDelete);
            await _dbContext.SaveChangesAsync();
            return todoListId;
        }

        public async Task<int> UpdateTodoList(TblTodoList todoList, int todoListId, IEnumerable<TblLabelMapping> mappings)
        {
            Log.Information("Going to hit database");
            var existingRecord = _dbContext.TblTodoList.Single(x => x.TodoListId == todoListId);
            existingRecord.ExpectedDate = todoList.ExpectedDate;
            existingRecord.ListName = todoList.ListName;

            var relatedLabelsData = _dbContext.TblLabelMapping.Where(x => x.RecordId == todoListId && x.TodoTypeId == 1);
            _dbContext.TblLabelMapping.RemoveRange(relatedLabelsData);
            await _dbContext.SaveChangesAsync();

            mappings.ToList().ForEach(x => x.RecordId = todoListId);
            _dbContext.TblLabelMapping.AddRange(mappings);
            await _dbContext.SaveChangesAsync();

            return todoListId;
        }

        public async Task<int> UpdatePatchTodoList(JsonPatchDocument todoList, int todoListId)
        {
            Log.Information("Going to hit database");
            var list = await _dbContext.TblTodoList.FindAsync(todoListId);
            if (list != null)
            {
                todoList.ApplyTo(list);
                await _dbContext.SaveChangesAsync();
            }
            return todoListId;
        }

        public List<TblLabel> GetListLabels(int todoListId)
        {
            int[] recordRelevantLabelsIds = _dbContext.TblLabelMapping.Where(x => x.RecordId == todoListId && x.TodoTypeId == 1).Select(x => x.LabelId).ToArray();
            var labels = _dbContext.TblLabel.Where(x => recordRelevantLabelsIds.Contains(x.LabelId)).ToList();
            return labels;
        }
    }
}
