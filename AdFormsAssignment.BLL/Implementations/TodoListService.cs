using AdFormAssignment.DAL.Contracts;
using AdFormAssignment.DAL.Entities;
using AdFormsAssignment.BLL.Contracts;
using AdFormsAssignment.DTO;
using Microsoft.AspNetCore.JsonPatch;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdFormsAssignment.BLL
{

    public class ToDoListService : ITodoListService
    {
        private readonly ITodoListDal _todoDAL;

        public ToDoListService(ITodoListDal todoDAL)
        {
            _todoDAL = todoDAL;
        }
        public async Task<TodoListDetail> GetToDoList(int todoListId, int userId)
        {
            return await _todoDAL.GetTodoList(todoListId, userId);
        }
        public async Task<IEnumerable<TodoListDetail>> GetAllTodoLists(int PageNumber, int PageSize, string SearchText, int userId)
        {
            Log.Information($"Going to hit DAL method with info = Pagenumber:{PageNumber},Pagesize:{PageSize},SearchText:{SearchText}, UserId:{userId} ");
            PageNumber = PageNumber == 0 ? 1 : PageNumber;
            PageSize = PageSize == 0 ? int.MaxValue : PageSize;
            return await _todoDAL.GetAllTodoLists(PageNumber, PageSize, SearchText, userId);
        }

        public async Task<int> CreateToDoList(TblTodoListExtension list, int userId)
        {
            Log.Information($"Going to hit DAL method");
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
                TblLabelMapping mapping = new TblLabelMapping();
                mapping.LabelId = labelId;
                mapping.RecordId = list.TodoListId;
                mapping.TodoTypeId = 1;
                labelMappings.Add(mapping);
            }

            return await _todoDAL.CreateTodoList(tblTodoList, labelMappings);
        }

        public async Task<int> DeleteTodoList(int todoListId)
        {
            Log.Information($"Going to hit DAL method");
            return await _todoDAL.DeleteTodoList(todoListId);
        }
        public async Task<int> UpdateToDoList(TblTodoListExtension todoList, int todoListId, int userId)
        {
            Log.Information($"Going to hit DAL method");

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
                TblLabelMapping mapping = new TblLabelMapping();
                mapping.LabelId = labelId;
                mapping.RecordId = todoList.TodoListId;
                mapping.TodoTypeId = 1;
                labelMappings.Add(mapping);
            }
            return await _todoDAL.UpdateTodoList(tblTodoList, todoListId, labelMappings);
        }

        public async Task<int> UpdatePatchTodoList(JsonPatchDocument todoList, int todoListId)
        {
            Log.Information($"Going to hit DAL method");
            return await _todoDAL.UpdatePatchTodoList(todoList, todoListId);
        }
    }
}

