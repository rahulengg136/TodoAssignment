using AdFormAssignment.DAL.Contracts;
using AdFormAssignment.DAL.Entities;
using AdFormsAssignment.DTO;
using Microsoft.AspNetCore.JsonPatch;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Transactions;

namespace AdFormAssignment.DAL.Implementations
{
    public class TodoItemDal : ITodoItemDal
    {
        private readonly MyProjectContext _dbContext;
        public TodoItemDal(MyProjectContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<TodoItemDetail> GetTodoItem(int todoItemId, int userId)
        {
            Log.Information($"Going to hit database");

            var data = _dbContext.tblTodoItem
                   .Join(_dbContext.tblTodoList, items => items.TodoListId, lists => lists.TodoListId, (items, lists) => new
                   {
                       items.Description,
                       items.ExpectedDate,
                       items.TodoItemId,
                       items.TodoListId,
                       items.UserId,
                       lists.ListName,

                   })
                   .SingleOrDefault(x => x.TodoItemId == todoItemId);

            if (data != null)
            {

                TodoItemDetail todoItemDetail = JsonSerializer.Deserialize<TodoItemDetail>(JsonSerializer.Serialize(data));
                int[] recordRelevantLabelsIds = _dbContext.tblLabelMapping.Where(x => x.RecordId == todoItemId && x.TodoTypeId == 2).Select(x => x.LabelId).ToArray();
                todoItemDetail.Labels = _dbContext.tblLabel.Where(x => recordRelevantLabelsIds.Contains(x.LabelId)).ToList();

                return Task.FromResult(todoItemDetail);
            }
            else
            {
                return Task.FromResult<TodoItemDetail>(null);
            }
        }

        public Task<IEnumerable<TodoItemDetail>> GetAllTodoItems(int PageNumber, int PageSize, string SearchText, int userId)
        {
            Log.Information($"Going to hit database");


            var data = _dbContext.tblTodoItem
                 .Join(_dbContext.tblTodoList, items => items.TodoListId, lists => lists.TodoListId, (items, lists) => new
                 {
                     items.Description,
                     items.ExpectedDate,
                     items.TodoItemId,
                     items.TodoListId,
                     items.UserId,
                     lists.ListName,

                 })
                 .Where(x => ((SearchText == null) || x.Description.Contains(SearchText)) && x.UserId == userId)
                   .Skip((PageNumber - 1) * PageSize).Take(PageSize);


            IEnumerable<TodoItemDetail> todoItemsDetail = JsonSerializer.Deserialize<IEnumerable<TodoItemDetail>>(JsonSerializer.Serialize(data));

            int[] allItemsId = data.Select(x => x.TodoItemId).ToArray();

            List<TblLabelMapping> mappingsOfAllItems = _dbContext.tblLabelMapping.Where(x => allItemsId.Contains(x.RecordId) && x.TodoTypeId == 2).ToList();
            int[] allRequiredLabelIds = _dbContext.tblLabelMapping.Where(x => allItemsId.Contains(x.RecordId) && x.TodoTypeId == 2).Select(x => x.LabelId).ToArray();
            List<TblLabel> allLabelsRequired = _dbContext.tblLabel.Where(x => allRequiredLabelIds.Contains(x.LabelId)).ToList();

            foreach (var itemDetail in todoItemsDetail)
            {
                int[] labelIdsOfCurrentItem = mappingsOfAllItems.Where(x => x.RecordId == itemDetail.TodoItemId).Select(x => x.LabelId).ToArray();
                itemDetail.Labels = allLabelsRequired.Where(x => labelIdsOfCurrentItem.Contains(x.LabelId)).ToList();
            }
            return Task.FromResult(todoItemsDetail);
        }

        public async Task<int> CreateTodoItem(TblTodoItem todoItem, IEnumerable<TblLabelMapping> mappings)
        {
            Log.Information($"Going to hit database");

            _dbContext.tblTodoItem.Add(todoItem);
            await _dbContext.SaveChangesAsync();

            mappings.ToList().ForEach(x => x.RecordId = todoItem.TodoItemId);
            _dbContext.tblLabelMapping.AddRange(mappings);
            await _dbContext.SaveChangesAsync();

            return todoItem.TodoItemId;

        }
        public async Task<int> DeleteTodoItem(int todoItemId)
        {
            Log.Information($"Going to hit database");
            var itemToDelete = _dbContext.tblTodoItem.Single(x => x.TodoItemId == todoItemId);
            _dbContext.tblTodoItem.Remove(itemToDelete);
            await _dbContext.SaveChangesAsync();
            return todoItemId;

        }

        public async Task<int> UpdateTodoItem(TblTodoItem todoItem, int todoItemId, IEnumerable<TblLabelMapping> mappings)
        {
            Log.Information($"Going to hit database");
            var existingRecord = _dbContext.tblTodoItem.Single(x => x.TodoItemId == todoItemId);
            existingRecord.ExpectedDate = todoItem.ExpectedDate;
            existingRecord.Description = todoItem.Description;

            var relatedLabelsData = _dbContext.tblLabelMapping.Where(x => x.RecordId == todoItemId && x.TodoTypeId == 2);
            _dbContext.tblLabelMapping.RemoveRange(relatedLabelsData);
            await _dbContext.SaveChangesAsync();

            mappings.ToList().ForEach(x => x.RecordId = todoItemId);
            _dbContext.tblLabelMapping.AddRange(mappings);
            await _dbContext.SaveChangesAsync();

            return todoItemId;
        }

        public async Task<int> UpdatePatchTodoItem(JsonPatchDocument todoItem, int todoItemId)
        {
            Log.Information($"Going to hit database");
            var item = await _dbContext.tblTodoItem.FindAsync(todoItemId);
            if (item != null)
            {
                todoItem.ApplyTo(item);
                await _dbContext.SaveChangesAsync();
            }
            return todoItemId;
        }
    }
}
