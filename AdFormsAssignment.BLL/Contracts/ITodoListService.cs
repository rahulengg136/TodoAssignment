using AdFormAssignment.DAL.Entities;
using AdFormAssignment.DAL.Entities.DTO;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdFormsAssignment.BLL.Contracts
{
    /// <summary>
    /// Contract for to do list service
    /// </summary>
    public interface ITodoListService
    {
        Task<int> CreateToDoList(TblTodoListExtension list, int userId);
        Task<TodoListDetail> GetToDoList(int todoListId, int userId);
        Task<IEnumerable<TodoListDetail>> GetAllTodoLists(int pageNumber, int pageSize, string searchText, int userId);
        Task<int> DeleteTodoList(int todoListId);
        Task<int> UpdateToDoList(TblTodoListExtension todoList, int todoListId, int userId);
        Task<int> UpdatePatchTodoList(JsonPatchDocument todoList, int todoListId);
        List<TblLabel> GetListLabels(int todoListId);
    }
}
