using AdFormAssignment.DAL.Entities;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdFormAssignment.DAL.Contracts
{
    /// <summary>
    /// Contract for to-do list data access
    /// </summary>
    public interface ITodoListDal
    {
        Task<int> CreateTodoList(TblTodoList todoList);
        Task<TblTodoList> GetTodoList(int todoListId, int userId);
        Task<IEnumerable<TblTodoList>> GetAllTodoLists(int PageNumber, int PageSize, string SearchText, int userId);
        Task<int> DeleteTodoList(int todoListId);
        Task<int> UpdateTodoList(TblTodoList todoList, int todoListId);
        Task<int> UpdatePatchTodoList(JsonPatchDocument todoList, int todoListId);
    }

}
