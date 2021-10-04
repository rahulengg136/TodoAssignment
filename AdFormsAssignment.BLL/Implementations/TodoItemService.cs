using AdFormAssignment.DAL.Contracts;
using AdFormAssignment.DAL.Entities;
using AdFormsAssignment.BLL.Contracts;
using AdFormsAssignment.DTO;
using Microsoft.AspNetCore.JsonPatch;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdFormsAssignment.BLL.Implementations
{
    public class TodoItemService : ITodoItemService
    {
        private readonly ITodoItemDal _todoDAL;

        public TodoItemService(ITodoItemDal todoDAL)
        {
            _todoDAL = todoDAL;
        }
        public async Task<int> CreateToDoItem(TblTodoItem todoItem)
        {
            Log.Information($"Going to hit DAL method");
            return await _todoDAL.CreateTodoItem(todoItem);
        }

        public async Task<int> DeleteTodoItem(int todoItemId)
        {
            Log.Information($"Going to hit DAL method");
            return await _todoDAL.DeleteTodoItem(todoItemId);
        }

        public async Task<IEnumerable<TodoItemDetail>> GetAllTodoItems(int PageNumber, int PageSize, string SearchText, int userId)
        {
            Log.Information($"Going to hit DAL method");
            PageNumber = PageNumber == 0 ? 1 : PageNumber;
            PageSize = PageSize == 0 ? int.MaxValue : PageSize;
            return await _todoDAL.GetAllTodoItems(PageNumber, PageSize, SearchText, userId);
        }

        public async Task<TodoItemDetail> GetToDoItem(int todoItemId, int userId)
        {
            Log.Information($"Going to hit DAL method");
            return await _todoDAL.GetTodoItem(todoItemId, userId);
        }

        public async Task<int> UpdatePatchTodoItem(JsonPatchDocument todoItem, int todoItemId)
        {
            Log.Information($"Going to hit DAL method");
            return await _todoDAL.UpdatePatchTodoItem(todoItem, todoItemId);
        }

        public async Task<int> UpdateToDoItem(TblTodoItem todoItem, int todoItemId)
        {
            Log.Information($"Going to hit DAL method");
            return await _todoDAL.UpdateTodoItem(todoItem, todoItemId);
        }
    }
}
