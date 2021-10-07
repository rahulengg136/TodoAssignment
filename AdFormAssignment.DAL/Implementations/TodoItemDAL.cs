using AdFormAssignment.DAL.Contracts;
using AdFormAssignment.DAL.Entities;
using AdFormAssignment.DAL.Entities.DTO;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdFormAssignment.DAL.Implementations
{
    public class TodoItemDal : ITodoItemDal
    {
        private readonly MyProjectContext _dbContext;
        public TodoItemDal(MyProjectContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TodoItemDetail> GetTodoItem(int todoItemId, int userId)
        {
            Log.Information("Going to hit database");

            var todoItemDetail = await _dbContext.TblTodoItem
                   .Join(_dbContext.TblTodoList, items => items.TodoListId, lists => lists.TodoListId, (items, lists) => new
                   TodoItemDetail()
                   {
                       Description = items.Description,
                       ExpectedDate = items.ExpectedDate,
                       TodoItemId = items.TodoItemId,
                       TodoListId = items.TodoListId,
                       UserId = items.UserId,
                       ListName = lists.ListName

                   })
                   .SingleOrDefaultAsync(x => x.TodoItemId == todoItemId);

            if (todoItemDetail != null)
            {
                int[] recordRelevantLabelsIds = _dbContext.TblLabelMapping.Where(x => x.RecordId == todoItemId && x.TodoTypeId == 2).Select(x => x.LabelId).ToArray();
                todoItemDetail.Labels = _dbContext.TblLabel.Where(x => recordRelevantLabelsIds.Contains(x.LabelId)).ToList();
                return todoItemDetail;
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<TodoItemDetail>> GetAllTodoItems(int pageNumber, int pageSize, string searchText, int userId)
        {
            Log.Information("Going to hit database");

            var todoItemsDetail = _dbContext.TblTodoItem
                 .Join(_dbContext.TblTodoList, items => items.TodoListId, lists => lists.TodoListId, (items, lists) => new
                TodoItemDetail()
                 {
                     Description = items.Description,
                     ExpectedDate = items.ExpectedDate,
                     TodoItemId = items.TodoItemId,
                     TodoListId = items.TodoListId,
                     UserId = items.UserId,
                     ListName = lists.ListName,

                 })
                 .Where(x => ((searchText == null) || x.Description.Contains(searchText)) && x.UserId == userId)
                   .Skip((pageNumber - 1) * pageSize).Take(pageSize);


            int[] allItemsId = todoItemsDetail.Select(x => x.TodoItemId).ToArray();
            List<TblLabelMapping> mappingsOfAllItems = await _dbContext.TblLabelMapping.Where(x => allItemsId.Contains(x.RecordId) && x.TodoTypeId == 2).ToListAsync();
            int[] allRequiredLabelIds = _dbContext.TblLabelMapping.Where(x => allItemsId.Contains(x.RecordId) && x.TodoTypeId == 2).Select(x => x.LabelId).ToArray();
            List<TblLabel> allLabelsRequired = await _dbContext.TblLabel.Where(x => allRequiredLabelIds.Contains(x.LabelId)).ToListAsync();

            var todoItemDetails = todoItemsDetail.ToList();
            foreach (var itemDetail in todoItemDetails)
            {
                int[] labelIdsOfCurrentItem = mappingsOfAllItems.Where(x => x.RecordId == itemDetail.TodoItemId).Select(x => x.LabelId).ToArray();
                itemDetail.Labels = allLabelsRequired.Where(x => labelIdsOfCurrentItem.Contains(x.LabelId)).ToList();
            }
            return todoItemDetails.AsEnumerable();
        }

        public async Task<int> CreateTodoItem(TblTodoItem todoItem, IEnumerable<TblLabelMapping> mappings)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    Log.Information("Going to hit database");

                    await _dbContext.TblTodoItem.AddAsync(todoItem);
                    await _dbContext.SaveChangesAsync();

                    var tblLabelMappings = mappings.ToList();
                    tblLabelMappings.ToList().ForEach(x => x.RecordId = todoItem.TodoItemId);
                    await _dbContext.TblLabelMapping.AddRangeAsync(tblLabelMappings);
                    await _dbContext.SaveChangesAsync();
                    transaction.Commit();

                    return todoItem.TodoItemId;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Log.Information($"Exception: {ex.StackTrace}");
                    throw;
                }
            }
        }

        public async Task<int> DeleteTodoItem(int todoItemId)
        {
            Log.Information("Going to hit database");
            var itemToDelete = await _dbContext.TblTodoItem.SingleAsync(x => x.TodoItemId == todoItemId);
            _dbContext.TblTodoItem.Remove(itemToDelete);
            await _dbContext.SaveChangesAsync();
            return todoItemId;
        }

        public async Task<int> UpdateTodoItem(TblTodoItem todoItem, int todoItemId,
            IEnumerable<TblLabelMapping> mappings)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    Log.Information("Going to hit database");
                    var existingRecord = _dbContext.TblTodoItem.Single(x => x.TodoItemId == todoItemId);
                    existingRecord.ExpectedDate = todoItem.ExpectedDate;
                    existingRecord.Description = todoItem.Description;

                    var relatedLabelsData =
                        _dbContext.TblLabelMapping.Where(x => x.RecordId == todoItemId && x.TodoTypeId == 2);
                    _dbContext.TblLabelMapping.RemoveRange(relatedLabelsData);
                    await _dbContext.SaveChangesAsync();

                    var tblLabelMappings = mappings.ToList();
                    tblLabelMappings.ToList().ForEach(x => x.RecordId = todoItemId);
                    await _dbContext.TblLabelMapping.AddRangeAsync(tblLabelMappings);
                    await _dbContext.SaveChangesAsync();
                    transaction.Commit();

                    return todoItemId;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Log.Information($"Exception: {ex.StackTrace}");
                    throw;
                }
            }
        }

        public async Task<int> UpdatePatchTodoItem(JsonPatchDocument todoItem, int todoItemId)
        {
            Log.Information("Going to hit database");
            var item = await _dbContext.TblTodoItem.FindAsync(todoItemId);
            if (item != null)
            {
                todoItem.ApplyTo(item);
                await _dbContext.SaveChangesAsync();
            }
            return todoItemId;
        }
        public List<TblLabel> GetItemLabels(int todoItemId)
        {
            int[] recordRelevantLabelsIds = _dbContext.TblLabelMapping.Where(x => x.RecordId == todoItemId && x.TodoTypeId == 2).Select(x => x.LabelId).ToArray();
            var labels = _dbContext.TblLabel.Where(x => recordRelevantLabelsIds.Contains(x.LabelId)).ToList();
            return labels;
        }
    }
}
