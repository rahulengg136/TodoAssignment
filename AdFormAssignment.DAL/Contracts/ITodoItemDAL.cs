using AdFormAssignment.DAL.Entities;
using AdFormAssignment.DAL.Entities.DTO;
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
        Task<int> CreateTodoItem(TblTodoItem todoItem, IEnumerable<TblLabelMapping> mappings);
        Task<TodoItemDetail> GetTodoItem(int todoItemId, int userId);
        Task<IEnumerable<TodoItemDetail>> GetAllTodoItems(int pageNumber, int pageSize, string searchText, int userId);
        Task<int> DeleteTodoItem(int todoItemId);
        Task<int> UpdateTodoItem(TblTodoItem todoItem, int todoItemId, IEnumerable<TblLabelMapping> mappings);
        Task<int> UpdatePatchTodoItem(JsonPatchDocument todoItem, int todoItemId);
        List<TblLabel> GetItemLabels(int todoItemId);
    }
}

