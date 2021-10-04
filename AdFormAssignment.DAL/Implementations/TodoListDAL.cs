using AdFormAssignment.DAL.Contracts;
using AdFormAssignment.DAL.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

        public Task<TblTodoList> GetTodoList(int todoListId, int userId)
        {
            return Task.FromResult(_dbContext.tblTodoList.SingleOrDefault(x => x.TodoListId == todoListId && x.UserId == userId));
        }

        public Task<IEnumerable<TblTodoList>> GetAllTodoLists(int PageNumber, int PageSize, string SearchText, int userId)
        {
            Log.Information("Going to hit database");
            return Task.FromResult(_dbContext.tblTodoList.Where(x => ((SearchText == null) || x.ListName.Contains(SearchText)) && x.UserId == userId).Skip((PageNumber - 1) * PageSize).Take(PageSize).AsEnumerable());
        }

        public async Task<int> CreateTodoList(TblTodoList todoList)
        {
            Log.Information("Going to hit database");
            _dbContext.tblTodoList.Add(todoList);
            await _dbContext.SaveChangesAsync();
            return todoList.TodoListId;
        }
        public async Task<int> DeleteTodoList(int todoListId)
        {
            Log.Information("Going to hit database");
            var listToDelete = _dbContext.tblTodoList.Single(x => x.TodoListId == todoListId);
            _dbContext.tblTodoList.Remove(listToDelete);
            await _dbContext.SaveChangesAsync();
            return todoListId;

        }

        public async Task<int> UpdateTodoList(TblTodoList todoList, int todoListId)
        {
            Log.Information("Going to hit database");
            var existingRecord = _dbContext.tblTodoList.Single(x => x.TodoListId == todoListId);
            existingRecord.ExpectedDate = todoList.ExpectedDate;
            existingRecord.LabelId = todoList.LabelId;
            existingRecord.ListName = todoList.ListName;

            await _dbContext.SaveChangesAsync();
            return todoListId;
        }

        public async Task<int> UpdatePatchTodoList(JsonPatchDocument todoList, int todoListId)
        {
            Log.Information("Going to hit database");
            var list = await _dbContext.tblTodoList.FindAsync(todoListId);
            if (list != null)
            {
                todoList.ApplyTo(list);
                await _dbContext.SaveChangesAsync();
            }
            return todoListId;

        }
    }
}
