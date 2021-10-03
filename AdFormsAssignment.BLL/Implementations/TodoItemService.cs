using AdFormAssignment.DAL.Contracts;
using AdFormAssignment.DAL.Entities;
using AdFormsAssignment.BLL.Contracts;
using Microsoft.AspNetCore.JsonPatch;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdFormsAssignment.BLL.Implementations
{
    public class TodoItemService : ITodoItemService
    {
        private readonly ITodoItemDAL _todoDAL;

        public TodoItemService(ITodoItemDAL todoDAL)
        {
            _todoDAL = todoDAL;
        }
        public async Task<int> CreateToDoItem(tblTodoItem todoItem)
        {
            Log.Information($"Going to hit DAL method");
            return await _todoDAL.CreateTodoItem(todoItem);
        }

        public async Task<int> DeleteTodoItem(int todoItemId)
        {
            Log.Information($"Going to hit DAL method");
            return await _todoDAL.DeleteTodoItem(todoItemId);
        }

        public async Task<IEnumerable<tblTodoItem>> GetAllTodoItems(int PageNumber, int PageSize, string SearchText, int userId)
        {
            Log.Information($"Going to hit DAL method");
            return await _todoDAL.GetAllTodoItems(PageNumber, PageSize, SearchText, userId);
        }

        public async Task<tblTodoItem> GetToDoItem(int todoItemId, int userId)
        {
            Log.Information($"Going to hit DAL method");
            return await _todoDAL.GetTodoItem(todoItemId, userId);
        }

        public async Task<int> UpdatePatchTodoItem(JsonPatchDocument todoItem, int todoItemId)
        {
            Log.Information($"Going to hit DAL method");
            return await _todoDAL.UpdatePatchTodoItem(todoItem, todoItemId);
        }

        public async Task<int> UpdateToDoItem(tblTodoItem todoItem, int todoItemId)
        {
            Log.Information($"Going to hit DAL method");
            return await _todoDAL.UpdateTodoItem(todoItem, todoItemId);
        }
    }
}
