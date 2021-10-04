using AdFormAssignment.DAL.Entities;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdFormsAssignment.BLL.Contracts
{
    /// <summary>
    /// Contract for todo item service
    /// </summary>
    public interface ITodoItemService
    {
        Task<int> CreateToDoItem(tblTodoItem todoItem);
        Task<tblTodoItem> GetToDoItem(int todoItemId, int userId);
        Task<IEnumerable<tblTodoItem>> GetAllTodoItems(int PageNumber, int PageSize, string SearchText, int userId);
        Task<int> DeleteTodoItem(int todoItemId);
        Task<int> UpdateToDoItem(tblTodoItem todoItem, int todoItemId);
        Task<int> UpdatePatchTodoItem(JsonPatchDocument todoItem, int todoItemId);
    }
}
