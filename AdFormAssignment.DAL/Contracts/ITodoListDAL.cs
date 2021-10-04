using AdFormAssignment.DAL.Entities;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdFormAssignment.DAL.Contracts
{
    /// <summary>
    /// Contract for todo list data access
    /// </summary>
    public interface ITodoListDAL
    {
        Task<int> CreateTodoList(tblTodoList todoList);
        Task<tblTodoList> GetTodoList(int todoListId, int userId);
        Task<IEnumerable<tblTodoList>> GetAllTodoLists(int PageNumber, int PageSize, string SearchText, int userId);
        Task<int> DeleteTodoList(int todoListId);
        Task<int> UpdateTodoList(tblTodoList todoList, int todoListId);
        Task<int> UpdatePatchTodoList(JsonPatchDocument todoList, int todoListId);
    }

}
