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
    public class ToDoListService : ITodoListService
    {
        private readonly ITodoListDal _todoDal;
        public ToDoListService(ITodoListDal todoDal)
        {
            _todoDal = todoDal;
        }
        public async Task<TodoListDetail> GetToDoList(int todoListId, int userId)
        {
            return await _todoDal.GetTodoList(todoListId, userId);
        }
        public async Task<IEnumerable<TodoListDetail>> GetAllTodoLists(int pageNumber, int pageSize, string searchText, int userId)
        {
            Log.Information($"Going to hit DAL method with info = PageNumber:{pageNumber},PageSize:{pageSize},SearchText:{searchText}, UserId:{userId} ");
            pageNumber = pageNumber == 0 ? 1 : pageNumber;
            pageSize = pageSize == 0 ? int.MaxValue : pageSize;
            return await _todoDal.GetAllTodoLists(pageNumber, pageSize, searchText, userId);
        }

        public async Task<int> CreateToDoList(TblTodoListExtension list, int userId)
        {
            Log.Information("Going to hit DAL method");
            // again convert it to tbl one and save 
            TblTodoList tblTodoList = new TblTodoList()
            {
                ListName = list.ListName,
                TodoListId = list.TodoListId,
                ExpectedDate = list.ExpectedDate,
                UserId = userId
            };
            List<TblLabelMapping> labelMappings = new List<TblLabelMapping>();
            foreach (int labelId in list.LabelIds)
            {
                TblLabelMapping mapping = new TblLabelMapping()
                {
                    LabelId = labelId,
                    RecordId = list.TodoListId,
                    TodoTypeId = 1
                };
                labelMappings.Add(mapping);
            }
            return await _todoDal.CreateTodoList(tblTodoList, labelMappings);
        }
        public async Task<int> DeleteTodoList(int todoListId)
        {
            Log.Information("Going to hit DAL method");
            return await _todoDal.DeleteTodoList(todoListId);
        }
        public async Task<int> UpdateToDoList(TblTodoListExtension todoList, int todoListId, int userId)
        {
            Log.Information("Going to hit DAL method");
            TblTodoList tblTodoList = new TblTodoList()
            {
                ExpectedDate = todoList.ExpectedDate,
                ListName = todoList.ListName,
                TodoListId = todoList.TodoListId,
                UserId = userId
            };

            List<TblLabelMapping> labelMappings = new List<TblLabelMapping>();
            foreach (int labelId in todoList.LabelIds)
            {
                TblLabelMapping mapping = new TblLabelMapping()
                {
                    LabelId = labelId,
                    RecordId = todoList.TodoListId,
                    TodoTypeId = 1
                };
                labelMappings.Add(mapping);
            }
            return await _todoDal.UpdateTodoList(tblTodoList, todoListId, labelMappings);
        }

        public async Task<int> UpdatePatchTodoList(JsonPatchDocument todoList, int todoListId)
        {
            Log.Information("Going to hit DAL method");
            return await _todoDal.UpdatePatchTodoList(todoList, todoListId);
        }

        public List<TblLabel> GetListLabels(int todoListId)
        {
            return _todoDal.GetListLabels(todoListId);
        }
    }
}

