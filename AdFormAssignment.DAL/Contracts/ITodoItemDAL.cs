﻿using AdFormAssignment.DAL.Entities;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdFormAssignment.DAL.Contracts
{
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
