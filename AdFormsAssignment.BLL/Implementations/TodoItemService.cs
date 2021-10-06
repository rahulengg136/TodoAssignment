using AdFormAssignment.DAL.Contracts;
using AdFormAssignment.DAL.Entities;
using AdFormsAssignment.BLL.Contracts;
using AdFormsAssignment.DTO;
using Microsoft.AspNetCore.JsonPatch;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdFormsAssignment.BLL.Implementations
{
    public class TodoItemService : ITodoItemService
    {
        private readonly ITodoItemDal _todoDAL;

        public TodoItemService(ITodoItemDal todoDAL)
        {
            _todoDAL = todoDAL;
        }
        public async Task<int> CreateToDoItem(TblTodoItemExtension todoItem, int userId)
        {
            Log.Information($"Going to hit DAL method");
            // again convert it to tbl one and save 
            TblTodoItem tblTodoItem = new TblTodoItem()
            {
                Description = todoItem.Description,
                ExpectedDate = todoItem.ExpectedDate,
                TodoItemId = todoItem.TodoItemId,
                TodoListId = todoItem.TodoListId,
                UserId = userId
            };

            List<TblLabelMapping> labelMappings = new List<TblLabelMapping>();
            foreach (int labelId in todoItem.LabelIds)
            {
                TblLabelMapping mapping = new TblLabelMapping()
                {
                    LabelId = labelId,
                    RecordId = todoItem.TodoItemId,
                    TodoTypeId = 2
                };
                labelMappings.Add(mapping);
            }
            return await _todoDAL.CreateTodoItem(tblTodoItem, labelMappings);
        }

        public async Task<int> DeleteTodoItem(int todoItemId)
        {
            Log.Information($"Going to hit DAL method");
            return await _todoDAL.DeleteTodoItem(todoItemId);
        }

        public async Task<IEnumerable<TodoItemDetail>> GetAllTodoItems(int PageNumber, int PageSize, string SearchText, int userId)
        {
            Log.Information($"Going to hit DAL method");
            PageNumber = PageNumber == 0 ? 1 : PageNumber;
            PageSize = PageSize == 0 ? int.MaxValue : PageSize;
            return await _todoDAL.GetAllTodoItems(PageNumber, PageSize, SearchText, userId);
        }
        public async Task<TodoItemDetail> GetToDoItem(int todoItemId, int userId)
        {
            Log.Information($"Going to hit DAL method");
            return await _todoDAL.GetTodoItem(todoItemId, userId);
        }

        public async Task<int> UpdatePatchTodoItem(JsonPatchDocument todoItem, int todoItemId)
        {
            Log.Information($"Going to hit DAL method");
            return await _todoDAL.UpdatePatchTodoItem(todoItem, todoItemId);
        }

        public async Task<int> UpdateToDoItem(TblTodoItemExtension todoItem, int todoItemId, int userId)
        {
            Log.Information($"Going to hit DAL method");
            TblTodoItem tblTodoItem = new TblTodoItem()
            {
                Description = todoItem.Description,
                ExpectedDate = todoItem.ExpectedDate,
                TodoItemId = todoItem.TodoItemId,
                TodoListId = todoItem.TodoListId,
                UserId = userId
            };
            List<TblLabelMapping> labelMappings = new List<TblLabelMapping>();
            foreach (int labelId in todoItem.LabelIds)
            {
                TblLabelMapping mapping = new TblLabelMapping()
                {
                    LabelId = labelId,
                    RecordId = todoItem.TodoItemId,
                    TodoTypeId = 2
                };
                labelMappings.Add(mapping);
            }
            return await _todoDAL.UpdateTodoItem(tblTodoItem, todoItemId, labelMappings);
        }

        public List<TblLabel> GetItemLabels(int todoItemId)
        {
            return _todoDAL.GetItemLabels(todoItemId);
        }
    }
}
