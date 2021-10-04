using AdFormAssignment.DAL.Entities;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdFormsAssignment.BLL.Contracts
{
    /// <summary>
    /// Contract for to-do item service
    /// </summary>
    public interface ITodoItemService
    {
        Task<int> CreateToDoItem(TblTodoItem todoItem);
        Task<TblTodoItem> GetToDoItem(int todoItemId, int userId);
        Task<IEnumerable<TblTodoItem>> GetAllTodoItems(int PageNumber, int PageSize, string SearchText, int userId);
        Task<int> DeleteTodoItem(int todoItemId);
        Task<int> UpdateToDoItem(TblTodoItem todoItem, int todoItemId);
        Task<int> UpdatePatchTodoItem(JsonPatchDocument todoItem, int todoItemId);
    }
}
