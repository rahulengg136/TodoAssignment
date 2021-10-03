using AdFormAssignment.DAL;
using AdFormAssignment.DAL.Contracts;
using AdFormAssignment.DAL.Entities;
using AdFormsAssignment.BLL.Contracts;
using Microsoft.AspNetCore.JsonPatch;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdFormsAssignment.BLL
{

    public class ToDoListService : ITodoListService
    {
        private readonly ITodoListDAL _todoDAL;

        public ToDoListService(ITodoListDAL todoDAL)
        {
            _todoDAL = todoDAL;
        }
        public async Task<tblTodoList> GetToDoList(int todoListId, int userId)
        {
            return await _todoDAL.GetTodoList(todoListId,userId);
        }
        public async Task<IEnumerable<tblTodoList>> GetAllTodoLists(int PageNumber, int PageSize, string SearchText, int userId)
        {
            Log.Information($"Going to hit DAL method with info = Pagenumber:{PageNumber},Pagesize:{PageSize},SearchText:{SearchText}, UserId:{userId} ");
            return await _todoDAL.GetAllTodoLists(PageNumber, PageSize, SearchText,userId);
        }

        public async Task<int> CreateToDoList(tblTodoList list)
        {
            Log.Information($"Going to hit DAL method");
            return await _todoDAL.CreateTodoList(list);
        }

        public async Task<int> DeleteTodoList(int todoListId)
        {
            Log.Information($"Going to hit DAL method");
            return await _todoDAL.DeleteTodoList(todoListId);
        }
        public async Task<int> UpdateToDoList(tblTodoList todoList, int todoListId)
        {
            Log.Information($"Going to hit DAL method");
            return await _todoDAL.UpdateTodoList(todoList, todoListId);
        }

        public async Task<int> UpdatePatchTodoList(JsonPatchDocument todoList, int todoListId)
        {
            Log.Information($"Going to hit DAL method");
            return await _todoDAL.UpdatePatchTodoList(todoList, todoListId);
        }
    }
}

