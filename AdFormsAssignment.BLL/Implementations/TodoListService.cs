using AdFormAssignment.DAL.Contracts;
using AdFormAssignment.DAL.Entities;
using AdFormsAssignment.BLL.Contracts;
using AdFormsAssignment.DTO;
using Microsoft.AspNetCore.JsonPatch;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdFormsAssignment.BLL
{

    public class ToDoListService : ITodoListService
    {
        private readonly ITodoListDal _todoDAL;

        public ToDoListService(ITodoListDal todoDAL)
        {
            _todoDAL = todoDAL;
        }
        public async Task<TodoListDetail> GetToDoList(int todoListId, int userId)
        {
            return await _todoDAL.GetTodoList(todoListId,userId);
        }
        public async Task<IEnumerable<TodoListDetail>> GetAllTodoLists(int PageNumber, int PageSize, string SearchText, int userId)
        {
            Log.Information($"Going to hit DAL method with info = Pagenumber:{PageNumber},Pagesize:{PageSize},SearchText:{SearchText}, UserId:{userId} ");
            PageNumber = PageNumber == 0 ? 1 : PageNumber;
            PageSize = PageSize == 0 ? int.MaxValue : PageSize;
            return await _todoDAL.GetAllTodoLists(PageNumber, PageSize, SearchText,userId);
        }

        public async Task<int> CreateToDoList(TblTodoList list)
        {
            Log.Information($"Going to hit DAL method");
            return await _todoDAL.CreateTodoList(list);
        }

        public async Task<int> DeleteTodoList(int todoListId)
        {
            Log.Information($"Going to hit DAL method");
            return await _todoDAL.DeleteTodoList(todoListId);
        }
        public async Task<int> UpdateToDoList(TblTodoList todoList, int todoListId)
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

