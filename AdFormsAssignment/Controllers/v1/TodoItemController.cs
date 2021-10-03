using AdFormAssignment.DAL.Common;
using AdFormAssignment.DAL.Entities;
using AdFormsAssignment.BLL.Contracts;
using AdFormsAssignment.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    /// <summary>
    /// This controller deals with CRUD operations of to-do item
    /// </summary>
    [Authorize]
    [Route("api/v1/[controller]/")]
    [ApiController]
    public class TodoItemController : ControllerBase
    {
        private readonly ITodoItemService _toDoService;
        private readonly IMapper _mapper;
        /// <summary>
        /// To do item controller
        /// </summary>
        /// <param name="toDoService"> To-Do service instance</param>
        /// <param name="mapper">Automapper instance</param>
        public TodoItemController(ITodoItemService toDoService, IMapper mapper)
        {
            _toDoService = toDoService;
            _mapper = mapper;
        }
        /// <summary>
        /// This method gives list of filtered task items
        /// </summary>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="SearchText">Any text that may present in description</param>
        /// <returns></returns>
        [HttpGet("allItems/{pageNumber}/{pageSize}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status200OK)]
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
        /// <summary>
        /// This method returns detail of a single to-do item
        /// </summary>
        /// <param name="todoItemId">To-Do item unique id</param>
        /// <returns></returns>
        [HttpGet("{todoItemId}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

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
        /// <summary>
        /// This method creates a new to-do item
        /// </summary>
        /// <param name="todoItem">New item data</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
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
        /// <summary>
        /// This method deletes to do item
        /// </summary>
        /// <param name="todoItemId">to-do item unique id</param>
        /// <returns></returns>
        [HttpDelete("{todoItemId}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]



        public async Task<IActionResult> DeleteTodoItem(int todoItemId)
        {
            using (LogContext.PushProperty("Correlation Id", RequestInfo.GetCorrelationId(HttpContext.Request)))
            {
                if (todoItemId == 0)
                    return BadRequest(new { message = "To do item id cannot be zero" });
                try
                {
                    var existingTodoItem = await _toDoService.GetToDoItem(todoItemId, int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
                    if (existingTodoItem == null)
                    {
                        return BadRequest("Resource not found with this unique id");
                    }
                    else
                    {
                        await _toDoService.DeleteTodoItem(todoItemId);
                        Log.Information($"Record deleted successfully : {todoItemId}");
                        return Ok(todoItemId);
                    }
                }
                catch (Exception exp)
                {
                    Log.Error($"Exception in method :{MicrosoftJson.Serialize(exp.StackTrace)}");
                    return StatusCode(500, exp.ToString());
                }
            }
        }
        /// <summary>
        /// This method updates any existing to-do item
        /// </summary>
        /// <param name="todoItemId">Unique to-do item id</param>
        /// <param name="todoItem">Item data</param>
        /// <returns></returns>
        [HttpPut("{todoItemId}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]

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

                    var existingTodoItem = await _toDoService.GetToDoItem(todoItemId, int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
                    if (existingTodoItem == null)
                    {
                        return BadRequest("Resource not found with this unique id");
                    }
                    else
                    {
                        var item = _mapper.Map<tblTodoItem>(todoItem);
                        await _toDoService.UpdateToDoItem(item, todoItemId);
                        Log.Information($"Record updated successfully. New record looks like: {MicrosoftJson.Serialize(item)}");
                        return Ok();
                    }
                }
                catch (Exception exp)
                {
                    Log.Error($"Exception in method :{MicrosoftJson.Serialize(exp.StackTrace)}");
                    return StatusCode(500, exp.ToString());
                }
            }

        }
        /// <summary>
        /// This method patches a record
        /// </summary>
        /// <param name="todoItemId">Unique to-do item id</param>
        /// <param name="todoItem">Patches info</param>
        /// <returns></returns>
        [HttpPatch("{todoItemId}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]

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

                    var existingTodoItem = await _toDoService.GetToDoItem(todoItemId, int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
                    if (existingTodoItem == null)
                    {
                        return BadRequest("Resource not found with this unique id");
                    }
                    else
                    {
                        await _toDoService.UpdatePatchTodoItem(todoItem, todoItemId);
                        Log.Information($"Record patched successfully. Info was: {MicrosoftJson.Serialize(todoItem.Operations)}");
                        return Ok();
                    }
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
