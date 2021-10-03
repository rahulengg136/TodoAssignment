using AdFormAssignment.DAL.Entities;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdFormsAssignment.BLL.Contracts
{
    public interface ITodoListService
    {
        Task<int> CreateToDoList(tblTodoList list);
        Task<tblTodoList> GetToDoList(int todoListId, int userId);
        Task<IEnumerable<tblTodoList>> GetAllTodoLists(int PageNumber, int PageSize, string SearchText, int userId);
        Task<int> DeleteTodoList(int todoListId);
        Task<int> UpdateToDoList(tblTodoList todoList, int todoListId);
        Task<int> UpdatePatchTodoList(JsonPatchDocument todoList, int todoListId);

    }
}
