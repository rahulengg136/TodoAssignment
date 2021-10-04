using AdFormAssignment.DAL.Entities;
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
        Task<int> CreateToDoList(TblTodoList list);
        Task<TblTodoList> GetToDoList(int todoListId, int userId);
        Task<IEnumerable<TblTodoList>> GetAllTodoLists(int PageNumber, int PageSize, string SearchText, int userId);
        Task<int> DeleteTodoList(int todoListId);
        Task<int> UpdateToDoList(TblTodoList todoList, int todoListId);
        Task<int> UpdatePatchTodoList(JsonPatchDocument todoList, int todoListId);

    }
}
