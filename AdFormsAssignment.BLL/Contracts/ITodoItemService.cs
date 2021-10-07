using AdFormAssignment.DAL.Entities;
using AdFormAssignment.DAL.Entities.DTO;
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
        Task<int> CreateToDoItem(TblTodoItemExtension todoItem, int userId);
        Task<TodoItemDetail> GetToDoItem(int todoItemId, int userId);
        Task<IEnumerable<TodoItemDetail>> GetAllTodoItems(int pageNumber, int pageSize, string searchText, int userId);
        Task<int> DeleteTodoItem(int todoItemId);
        Task<int> UpdateToDoItem(TblTodoItemExtension todoItem, int todoItemId, int userId);
        Task<int> UpdatePatchTodoItem(JsonPatchDocument todoItem, int todoItemId);
        List<TblLabel> GetItemLabels(int todoItemId);
    }
}
