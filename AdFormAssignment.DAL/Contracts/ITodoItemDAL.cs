using AdFormAssignment.DAL.Entities;
using AdFormsAssignment.DTO;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdFormAssignment.DAL.Contracts
{
    /// <summary>
    /// Contracts for to-do items data access
    /// </summary>
    public interface ITodoItemDal
    {
        Task<int> CreateTodoItem(TblTodoItem todoItem);
        Task<TodoItemDetail> GetTodoItem(int todoItemId,int userId);
        Task<IEnumerable<TodoItemDetail>> GetAllTodoItems(int PageNumber, int PageSize, string SearchText,int userId);
        Task<int> DeleteTodoItem(int todoItemId);
        Task<int> UpdateTodoItem(TblTodoItem todoItem, int todoItemId);
        Task<int> UpdatePatchTodoItem(JsonPatchDocument todoItem, int todoItemId);
    }
}

