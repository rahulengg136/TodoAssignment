using AdFormAssignment.DAL.Common;
using AdFormAssignment.DAL.Entities;
using AdFormAssignment.DAL.Entities.DTO;
using AdFormsAssignment.BLL.Contracts;
using AdFormsAssignment.DTO.Common;
using AdFormsAssignment.DTO.GetDto;
using AdFormsAssignment.DTO.PostDto;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Context;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MicrosoftJson = System.Text.Json.JsonSerializer;

namespace AdFormsAssignment.Controllers.v1
{
    /// <summary>
    /// This controller deals with CRUD operations of to-do item
    /// </summary>
    [Authorize]
    [Route("api/v1/[controller]/")]
    [ApiController]
    public class ToDoItemController : ControllerBase
    {
        private readonly ITodoItemService _toDoService;
        private readonly IMapper _mapper;
        /// <summary>
        /// To do item controller
        /// </summary>
        /// <param name="toDoService"> To-Do service instance</param>
        /// <param name="mapper">AutoMapper instance</param>
        public ToDoItemController(ITodoItemService toDoService, IMapper mapper)
        {
            _toDoService = toDoService;
            _mapper = mapper;
        }
        /// <summary>
        /// This method gives list of filtered task items
        /// </summary>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="searchText">Any text that may present in description</param>
        /// <returns>Returns list of items</returns>
        [HttpGet("Items")]
        [ProducesResponseType(typeof(UnauthorizedInfo), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(IEnumerable<ReadTodoItemDto>), 200)]
        public async Task<IActionResult> GetAllTaskItems(int pageNumber, int pageSize, string searchText)
        {
            using (LogContext.PushProperty("Correlation Id", RequestInfo.GetCorrelationId(HttpContext.Request)))
            {
                var allTodoItems = await _toDoService.GetAllTodoItems(pageNumber, pageSize, searchText, int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
                var todoItemDetails = allTodoItems.ToList();
                Log.Information($"Filtered todo items :{MicrosoftJson.Serialize(todoItemDetails)}");
                if (todoItemDetails.Any())
                    return Ok(_mapper.Map<IEnumerable<ReadTodoItemDto>>(allTodoItems));
                else
                    return NoContent();
            }
        }
        /// <summary>
        /// This method returns detail of a single to-do item
        /// </summary>
        /// <param name="todoItemId">To-Do item unique id</param>
        /// <returns>Returns detail of single to-do item</returns>
        [HttpGet("{todoItemId}")]
        [ProducesResponseType(typeof(UnauthorizedInfo), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ReadTodoItemDto), 200)]
        [ProducesResponseType(typeof(BadRequestInfo), 400)]

        public async Task<IActionResult> GetTodoItem(int todoItemId)
        {
            using (LogContext.PushProperty("Correlation Id", RequestInfo.GetCorrelationId(HttpContext.Request)))
            {
                if (todoItemId == 0)
                {
                    return BadRequest(new { message = "To do item Id cannot be zero or null" });
                }
                var todoItem = await _toDoService.GetToDoItem(todoItemId, int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
                Log.Information($"Filtered todo item :{MicrosoftJson.Serialize(todoItem)}");
                if (todoItem != null)
                {
                    return Ok(_mapper.Map<ReadTodoItemDto>(todoItem));
                }
                else
                    return NoContent();
            }
        }
        /// <summary>
        /// This method creates a new to-do item
        /// </summary>
        /// <param name="todoItem">New item data</param>
        /// <returns>Returns success if item gets created successfully</returns>
        [HttpPost]
        [ProducesResponseType(typeof(UnauthorizedInfo), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BadRequestInfo), 400)]
        [ProducesResponseType(typeof(ReadTodoItemDto), 201)]

        public async Task<IActionResult> CreateTodoItem([FromBody] CreateTodoItemDto todoItem)
        {
            using (LogContext.PushProperty("Correlation Id", RequestInfo.GetCorrelationId(HttpContext.Request)))
            {
                if (string.IsNullOrEmpty(todoItem.Description))
                {
                    return BadRequest(new { message = "Description cannot be null or empty" });
                }
                var item = _mapper.Map<TblTodoItemExtension>(todoItem);
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                int newRecordId = await _toDoService.CreateToDoItem(item, userId);
                Log.Information($"Record created successfully :{MicrosoftJson.Serialize(item)}");
                var newlyCreatedRecord = await _toDoService.GetToDoItem(newRecordId, int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
                return Created($"~/api/v1/TodoItem/{newRecordId}", newlyCreatedRecord);
            }
        }
        /// <summary>
        /// This method deletes to do item
        /// </summary>
        /// <param name="todoItemId">to-do item unique id</param>
        /// <returns>Returns success if item gets deleted successfully</returns>
        [HttpDelete("{todoItemId}")]
        [ProducesResponseType(typeof(UnauthorizedInfo), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BadRequestInfo), 400)]
        [ProducesResponseType(typeof(TodoItemDetail), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteTodoItem(int todoItemId)
        {
            using (LogContext.PushProperty("Correlation Id", RequestInfo.GetCorrelationId(HttpContext.Request)))
            {
                if (todoItemId == 0)
                    return BadRequest(new { message = "To do item id cannot be zero" });
                var existingTodoItem = await _toDoService.GetToDoItem(todoItemId, int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
                if (existingTodoItem == null)
                {
                    return BadRequest(new { Message = "Resource not found with this unique id" });
                }
                else
                {
                    await _toDoService.DeleteTodoItem(todoItemId);
                    Log.Information($"Record deleted successfully : {todoItemId}");
                    return Ok(existingTodoItem);
                }
            }
        }
        /// <summary>
        /// This method updates any existing to-do item
        /// </summary>
        /// <param name="todoItemId">Unique to-do item id</param>
        /// <param name="todoItem">Item data</param>
        /// <returns>Returns success if item gets updated successfully</returns>
        [HttpPut("{todoItemId}")]
        [ProducesResponseType(typeof(UnauthorizedInfo), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BadRequestInfo), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(TodoItemDetail), StatusCodes.Status200OK)]

        public async Task<IActionResult> UpdateTodoItem(int todoItemId, [FromBody] CreateTodoItemDto todoItem)
        {
            using (LogContext.PushProperty("Correlation Id", RequestInfo.GetCorrelationId(HttpContext.Request)))
            {
                if (todoItemId == 0)
                {
                    return BadRequest(new { message = "To do item id cannot be zero" });
                }

                var existingTodoItem = await _toDoService.GetToDoItem(todoItemId, int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
                if (existingTodoItem == null)
                {
                    return BadRequest(new { Message = "Resource not found with this unique id" });
                }
                else
                {
                    var item = _mapper.Map<TblTodoItemExtension>(todoItem);
                    var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                    await _toDoService.UpdateToDoItem(item, todoItemId, userId);
                    Log.Information($"Record updated successfully. New record looks like: {MicrosoftJson.Serialize(item)}");
                    return Ok(await _toDoService.GetToDoItem(todoItemId, int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value)));
                }
            }
        }


        /// <summary>
        /// This method patches a record
        /// </summary>
        /// <param name="todoItemId">Unique to-do item id</param>
        /// <param name="todoItem">Patches info</param>
        /// <returns>Returns success if item gets patched successfully</returns>
        [HttpPatch("{todoItemId}")]
        [ProducesResponseType(typeof(UnauthorizedInfo), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BadRequestInfo), 400)]
        [ProducesResponseType(typeof(TodoItemDetail), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateTodoItemPatch(int todoItemId, [FromBody] JsonPatchDocument todoItem)
        {
            using (LogContext.PushProperty("Correlation Id", RequestInfo.GetCorrelationId(HttpContext.Request)))
            {
                if (todoItemId == 0)
                {
                    return BadRequest(new { message = "To do item id cannot be zero" });
                }
                var existingTodoItem = await _toDoService.GetToDoItem(todoItemId, int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
                if (existingTodoItem == null)
                {
                    return BadRequest(new { Message = "Resource not found with this unique id" });
                }
                else
                {
                    await _toDoService.UpdatePatchTodoItem(todoItem, todoItemId);
                    Log.Information($"Record patched successfully. Info was: {MicrosoftJson.Serialize(todoItem.Operations)}");
                    return Ok(await _toDoService.GetToDoItem(todoItemId, int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value)));
                }
            }
        }
    }
}

