using AdFormAssignment.DAL.Common;
using AdFormAssignment.DAL.Entities;
using AdFormsAssignment.BLL.Contracts;
using AdFormsAssignment.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Context;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MicrosoftJson = System.Text.Json.JsonSerializer;

namespace AdFormsAssignment.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]/")]
    [ApiController]
    public class TodoItemController : ControllerBase
    {
        private readonly ITodoItemService _toDoService;
        private readonly IMapper _mapper;
        public TodoItemController(ITodoItemService toDoService, IMapper mapper)
        {
            _toDoService = toDoService;
            _mapper = mapper;
        }

        [HttpGet("allItems/{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetAllTaskItems(int pageNumber, int pageSize, string SearchText)
        {
            using (LogContext.PushProperty("Correlation Id", RequestInfo.GetCorrelationId(HttpContext.Request)))
            {
                try
                {
                    var allTodoItems = await _toDoService.GetAllTodoItems(pageNumber, pageSize, SearchText, int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
                    Log.Information($"Filtered todo items :{MicrosoftJson.Serialize(allTodoItems)}");
                    if (allTodoItems.Count() > 0)
                        return Ok(allTodoItems);
                    else
                        return NoContent();

                }
                catch (Exception exp)
                {
                    Log.Error($"Exception in method :{MicrosoftJson.Serialize(exp.StackTrace)}");
                    return StatusCode(500, exp.ToString());
                }
            }
        }
        [HttpGet("{todoItemId}")]
        public async Task<IActionResult> GetTodoItem(int todoItemId)
        {
            using (LogContext.PushProperty("Correlation Id", RequestInfo.GetCorrelationId(HttpContext.Request)))
            {
                if (todoItemId == 0)
                {
                    return BadRequest(new { message = "To do item Id cannot be zero or null" });
                }
                try
                {
                    var todoItem = await _toDoService.GetToDoItem(todoItemId, int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
                    Log.Information($"Filtered todo item :{MicrosoftJson.Serialize(todoItem)}");
                    if (todoItem != null)
                        return Ok(todoItem);
                    else
                        return NoContent();
                }
                catch (Exception exp)
                {
                    Log.Error($"Exception in method :{MicrosoftJson.Serialize(exp.StackTrace)}");
                    return StatusCode(500, exp.ToString());
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTodoItem([FromBody] TodoItemDto todoItem)
        {
            using (LogContext.PushProperty("Correlation Id", RequestInfo.GetCorrelationId(HttpContext.Request)))
            {
                if (string.IsNullOrEmpty(todoItem.Description))
                {
                    return BadRequest(new { message = "Description cannot be null or empty" });
                }
                try
                {
                    var item = _mapper.Map<tblTodoItem>(todoItem);
                    int newRecordId = await _toDoService.CreateToDoItem(item);
                    Log.Information($"Record created successfully :{MicrosoftJson.Serialize(item)}");
                    return Created($"~/api/v1/TodoItem/{newRecordId}", item);
                }
                catch (Exception exp)
                {
                    Log.Error($"Exception in method :{MicrosoftJson.Serialize(exp.StackTrace)}");
                    return StatusCode(500, exp.ToString());
                }
            }
        }

        [HttpDelete("{todoItemId}")]
        public async Task<IActionResult> DeleteTodoItem(int todoItemId)
        {
            using (LogContext.PushProperty("Correlation Id", RequestInfo.GetCorrelationId(HttpContext.Request)))
            {
                if (todoItemId == 0)
                    return BadRequest(new { message = "To do item id cannot be zero" });
                try
                {
                    await _toDoService.DeleteTodoItem(todoItemId);
                    Log.Information($"Record deleted successfully : {todoItemId}");
                    return Ok(todoItemId);
                }
                catch (Exception exp)
                {
                    Log.Error($"Exception in method :{MicrosoftJson.Serialize(exp.StackTrace)}");
                    return StatusCode(500, exp.ToString());
                }
            }
        }
        [HttpPut("{todoItemId}")]
        public async Task<IActionResult> UpdateTodoItem(int todoItemId, [FromBody] TodoItemDto todoItem)
        {
            using (LogContext.PushProperty("Correlation Id", RequestInfo.GetCorrelationId(HttpContext.Request)))
            {
                if (todoItemId == 0)
                {
                    return BadRequest(new { message = "To do item id cannot be zero" });
                }
                try
                {
                    var item = _mapper.Map<tblTodoItem>(todoItem);
                    await _toDoService.UpdateToDoItem(item, todoItemId);
                    Log.Information($"Record updated successfully. New record looks like: {MicrosoftJson.Serialize(item)}");
                    return Ok();
                }
                catch (Exception exp)
                {
                    Log.Error($"Exception in method :{MicrosoftJson.Serialize(exp.StackTrace)}");
                    return StatusCode(500, exp.ToString());
                }
            }

            }
            [HttpPatch("{todoItemId}")]
            public async Task<IActionResult> UpdateTodoItemPatch(int todoItemId, [FromBody] JsonPatchDocument todoItem)
            {
            using (LogContext.PushProperty("Correlation Id", RequestInfo.GetCorrelationId(HttpContext.Request)))
            {
                if (todoItemId == 0)
                {
                    return BadRequest(new { message = "To do item id cannot be zero" });
                }
                try
                {
                    await _toDoService.UpdatePatchTodoItem(todoItem, todoItemId);
                    Log.Information($"Record patched successfully. Info was: {MicrosoftJson.Serialize(todoItem.Operations)}");
                    return Ok();
                }
                catch (Exception exp)
                {
                    Log.Error($"Exception in method :{MicrosoftJson.Serialize(exp.StackTrace)}");
                    return StatusCode(500, exp.ToString());
                }
            }
            }


        }
    }
