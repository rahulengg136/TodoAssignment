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
            var data = _dbContext.tblTodoList
                   .Join(_dbContext.tblLabel, lists => lists.LabelId, labels => labels.LabelId, (lists, labels) => new
                   {
                      lists.ExpectedDate,
                      lists.LabelId,
                      lists.ListName,
                      lists.TodoListId,
                      lists.UserId,
                       labels.LabelName
                   })
                  .Single(x => x.TodoListId == todoListId);

            TodoListDetail todoListDetail = JsonSerializer.Deserialize<TodoListDetail>(JsonSerializer.Serialize(data));
            return Task.FromResult(todoListDetail);
        }

        public Task<IEnumerable<TodoListDetail>> GetAllTodoLists(int PageNumber, int PageSize, string SearchText, int userId)
        {
            Log.Information("Going to hit database");
            var data = _dbContext.tblTodoList
                  .Join(_dbContext.tblLabel, lists => lists.LabelId, labels => labels.LabelId, (lists, labels) => new
                  {
                      lists.ExpectedDate,
                      lists.LabelId,
                      lists.ListName,
                      lists.TodoListId,
                      lists.UserId,
                      labels.LabelName
                  })
                 .Where(x => ((SearchText == null) || x.ListName.Contains(SearchText)) && x.UserId == userId).Skip((PageNumber - 1) * PageSize).Take(PageSize).AsEnumerable();

            IEnumerable<TodoListDetail> todoListsDetail = JsonSerializer.Deserialize<IEnumerable<TodoListDetail>>(JsonSerializer.Serialize(data));
            return Task.FromResult(todoListsDetail);
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
