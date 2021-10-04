using AdFormAssignment.DAL.Contracts;
using AdFormAssignment.DAL.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdFormAssignment.DAL.Implementations
{
    public class TodoItemDal : ITodoItemDal
    {
        private readonly MyProjectContext _dbContext;
        public TodoItemDal(MyProjectContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<TblTodoItem> GetTodoItem(int todoItemId, int userId)
        {
            Log.Information($"Going to hit database");
            return Task.FromResult(_dbContext.tblTodoItem.SingleOrDefault(x => x.TodoItemId == todoItemId && x.UserId == userId));
        }

        public Task<IEnumerable<TblTodoItem>> GetAllTodoItems(int PageNumber, int PageSize, string SearchText, int userId)
        {
            Log.Information($"Going to hit database");
            return Task.FromResult(_dbContext.tblTodoItem.Where(x => ((SearchText == null) || x.Description.Contains(SearchText)) && x.UserId == userId).Skip((PageNumber - 1) * PageSize).Take(PageSize).AsEnumerable());
        }

        public async Task<int> CreateTodoItem(TblTodoItem todoItem)
        {
            Log.Information($"Going to hit database");
            _dbContext.tblTodoItem.Add(todoItem);
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

        public async Task<int> UpdateTodoItem(TblTodoItem todoItem, int todoItemId)
        {
            Log.Information($"Going to hit database");
            var existingRecord = _dbContext.tblTodoItem.Single(x => x.TodoItemId == todoItemId);
            existingRecord.ExpectedDate = todoItem.ExpectedDate;
            existingRecord.LabelId = todoItem.LabelId;
            existingRecord.Description = todoItem.Description;

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
