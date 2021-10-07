using AdFormAssignment.DAL.Contracts;
using AdFormAssignment.DAL.Entities;
using AdFormAssignment.DAL.Entities.DTO;
using AdFormsAssignment.BLL.Contracts;
using Microsoft.AspNetCore.JsonPatch;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdFormsAssignment.BLL.Implementations
{
    public class TodoItemService : ITodoItemService
    {
        private readonly ITodoItemDal _todoDal;

        public TodoItemService(ITodoItemDal todoDal)
        {
            _todoDal = todoDal;
        }
        public async Task<int> CreateToDoItem(TblTodoItemExtension todoItem, int userId)
        {
            Log.Information("Going to hit DAL method");
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
            return await _todoDal.CreateTodoItem(tblTodoItem, labelMappings);
        }

        public async Task<int> DeleteTodoItem(int todoItemId)
        {
            Log.Information("Going to hit DAL method");
            return await _todoDal.DeleteTodoItem(todoItemId);
        }

        public async Task<IEnumerable<TodoItemDetail>> GetAllTodoItems(int pageNumber, int pageSize, string searchText, int userId)
        {
            Log.Information("Going to hit DAL method");
            pageNumber = pageNumber == 0 ? 1 : pageNumber;
            pageSize = pageSize == 0 ? int.MaxValue : pageSize;
            return await _todoDal.GetAllTodoItems(pageNumber, pageSize, searchText, userId);
        }
        public async Task<TodoItemDetail> GetToDoItem(int todoItemId, int userId)
        {
            Log.Information("Going to hit DAL method");
            return await _todoDal.GetTodoItem(todoItemId, userId);
        }

        public async Task<int> UpdatePatchTodoItem(JsonPatchDocument todoItem, int todoItemId)
        {
            Log.Information("Going to hit DAL method");
            return await _todoDal.UpdatePatchTodoItem(todoItem, todoItemId);
        }

        public async Task<int> UpdateToDoItem(TblTodoItemExtension todoItem, int todoItemId, int userId)
        {
            Log.Information("Going to hit DAL method");
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
            return await _todoDal.UpdateTodoItem(tblTodoItem, todoItemId, labelMappings);
        }

        public List<TblLabel> GetItemLabels(int todoItemId)
        {
            return _todoDal.GetItemLabels(todoItemId);
        }
    }
}
