using AdFormAssignment.DAL.Contracts;
using AdFormAssignment.DAL.Entities;
using AdFormAssignment.DAL.Entities.DTO;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdFormAssignment.DAL.Implementations
{
    public class TodoListDal : ITodoListDal
    {
        private readonly MyProjectContext _dbContext;
        private readonly IMapper _mapper;
        public TodoListDal(MyProjectContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<TodoListDetail> GetTodoList(int todoListId, int userId)
        {
            var data = await _dbContext.TblTodoList.SingleOrDefaultAsync(x => x.TodoListId == todoListId);
            if (data != null)
            {
                TodoListDetail todoListDetail = _mapper.Map<TodoListDetail>(data);
                int[] recordRelevantLabelsIds = await _dbContext.TblLabelMapping.Where(x => x.RecordId == todoListId && x.TodoTypeId == 1).Select(x => x.LabelId).ToArrayAsync();
                todoListDetail.Labels = await _dbContext.TblLabel.Where(x => recordRelevantLabelsIds.Contains(x.LabelId)).ToListAsync();
                return todoListDetail;
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<TodoListDetail>> GetAllTodoLists(int pageNumber, int pageSize, string searchText, int userId)
        {
            Log.Information("Going to hit database");
            var data = _dbContext.TblTodoList.Where(x => ((searchText == null) || x.ListName.Contains(searchText)) && x.UserId == userId).Skip((pageNumber - 1) * pageSize).Take(pageSize).AsEnumerable();

            var tblTodoLists = data.ToList();
            var todoListsDetail = _mapper.Map<IEnumerable<TodoListDetail>>(data.AsEnumerable());

            int[] allToDoListsId = tblTodoLists.Select(x => x.TodoListId).ToArray();

            List<TblLabelMapping> mappingsOfAllLists = await _dbContext.TblLabelMapping.Where(x => allToDoListsId.Contains(x.RecordId) && x.TodoTypeId == 1).ToListAsync();
            int[] allRequiredLabelIds = await _dbContext.TblLabelMapping.Where(x => allToDoListsId.Contains(x.RecordId) && x.TodoTypeId == 1).Select(x => x.LabelId).ToArrayAsync();
            List<TblLabel> allLabelsRequired = await _dbContext.TblLabel.Where(x => allRequiredLabelIds.Contains(x.LabelId)).ToListAsync();

            var todoListDetails = todoListsDetail.ToList();
            foreach (var listDetail in todoListDetails)
            {
                int[] labelIdsOfCurrentList = mappingsOfAllLists.Where(x => x.RecordId == listDetail.TodoListId).Select(x => x.LabelId).ToArray();
                listDetail.Labels = allLabelsRequired.Where(x => labelIdsOfCurrentList.Contains(x.LabelId)).ToList();
            }
            return todoListDetails.AsEnumerable();
        }

        public async Task<int> CreateTodoList(TblTodoList todoList, IEnumerable<TblLabelMapping> mappings)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    Log.Information("Going to hit database");
                    _dbContext.TblTodoList.Add(todoList);
                    await _dbContext.SaveChangesAsync();

                    var tblLabelMappings = mappings.ToList();
                    tblLabelMappings.ToList().ForEach(x => x.RecordId = todoList.TodoListId);
                    _dbContext.TblLabelMapping.AddRange(tblLabelMappings);
                    await _dbContext.SaveChangesAsync();
                    transaction.Commit();
                    return todoList.TodoListId;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Log.Information($"Exception: {ex.StackTrace}");
                    throw;
                }
            }
        }
        public async Task<int> DeleteTodoList(int todoListId)
        {
            Log.Information("Going to hit database");
            var listToDelete = _dbContext.TblTodoList.Single(x => x.TodoListId == todoListId);
            _dbContext.TblTodoList.Remove(listToDelete);
            await _dbContext.SaveChangesAsync();
            return todoListId;
        }

        public async Task<int> UpdateTodoList(TblTodoList todoList, int todoListId, IEnumerable<TblLabelMapping> mappings)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    Log.Information("Going to hit database");
                    var existingRecord = _dbContext.TblTodoList.Single(x => x.TodoListId == todoListId);
                    existingRecord.ExpectedDate = todoList.ExpectedDate;
                    existingRecord.ListName = todoList.ListName;

                    var relatedLabelsData = _dbContext.TblLabelMapping.Where(x => x.RecordId == todoListId && x.TodoTypeId == 1);
                    _dbContext.TblLabelMapping.RemoveRange(relatedLabelsData);
                    await _dbContext.SaveChangesAsync();

                    var tblLabelMappings = mappings.ToList();
                    tblLabelMappings.ForEach(x => x.RecordId = todoListId);
                    _dbContext.TblLabelMapping.AddRange(tblLabelMappings);
                    await _dbContext.SaveChangesAsync();
                    transaction.Commit();

                    return todoListId;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Log.Information($"Exception: {ex.StackTrace}");
                    throw;
                }
            }
        }

        public async Task<int> UpdatePatchTodoList(JsonPatchDocument todoList, int todoListId)
        {
            Log.Information("Going to hit database");
            var list = await _dbContext.TblTodoList.FindAsync(todoListId);
            if (list != null)
            {
                todoList.ApplyTo(list);
                await _dbContext.SaveChangesAsync();
            }
            return todoListId;
        }

        public List<TblLabel> GetListLabels(int todoListId)
        {
            int[] recordRelevantLabelsIds = _dbContext.TblLabelMapping.Where(x => x.RecordId == todoListId && x.TodoTypeId == 1).Select(x => x.LabelId).ToArray();
            var labels = _dbContext.TblLabel.Where(x => recordRelevantLabelsIds.Contains(x.LabelId)).ToList();
            return labels;
        }
    }
}
