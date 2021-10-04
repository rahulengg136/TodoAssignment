using AdFormAssignment.DAL.Entities;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdFormAssignment.DAL.Contracts
{
    /// <summary>
    /// Contracts for todo items data access
    /// </summary>
    public interface ITodoItemDAL
    {
        Task<int> CreateTodoItem(tblTodoItem todoItem);
        Task<tblTodoItem> GetTodoItem(int todoItemId,int userId);
        Task<IEnumerable<tblTodoItem>> GetAllTodoItems(int PageNumber, int PageSize, string SearchText,int userId);
        Task<int> DeleteTodoItem(int todoItemId);
        Task<int> UpdateTodoItem(tblTodoItem todoItem, int todoItemId);
        Task<int> UpdatePatchTodoItem(JsonPatchDocument todoItem, int todoItemId);
    }
}

