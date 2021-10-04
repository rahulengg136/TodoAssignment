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
using System.Collections;
using System.Collections.Generic;
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
    public class ToDoItemController : ControllerBase
    {
        private readonly ITodoItemService _toDoService;
        private readonly IMapper _mapper;
        /// <summary>
        /// To do item controller
        /// </summary>
        /// <param name="toDoService"> To-Do service instance</param>
        /// <param name="mapper">Automapper instance</param>
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
        /// <param name="SearchText">Any text that may present in description</param>
        /// <returns>Returns list of items</returns>
        [HttpGet("Items")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(IEnumerable<ReadTodoItemDto>), 200)]
        public async Task<IActionResult> GetAllTaskItems(int pageNumber, int pageSize, string SearchText)
        {
            using (LogContext.PushProperty("Correlation Id", RequestInfo.GetCorrelationId(HttpContext.Request)))
            {

                var allTodoItems = await _toDoService.GetAllTodoItems(pageNumber, pageSize, SearchText, int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
                Log.Information($"Filtered todo items :{MicrosoftJson.Serialize(allTodoItems)}");
                if (allTodoItems.Any())
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
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ReadTodoItemDto), 200)]
        [ProducesResponseType(typeof(string), 400)]

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
                    return Ok(_mapper.Map<ReadTodoItemDto>(todoItem));
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
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(ReadTodoItemDto), 201)]

        public async Task<IActionResult> CreateTodoItem([FromBody] CreateTodoItemDto todoItem)
        {
            using (LogContext.PushProperty("Correlation Id", RequestInfo.GetCorrelationId(HttpContext.Request)))
            {
                if (string.IsNullOrEmpty(todoItem.Description))
                {
                    return BadRequest(new { message = "Description cannot be null or empty" });
                }

                var item = _mapper.Map<TblTodoItem>(todoItem);
                item.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                int newRecordId = await _toDoService.CreateToDoItem(item);
                Log.Information($"Record created successfully :{MicrosoftJson.Serialize(item)}");
                return Created($"~/api/v1/TodoItem/{newRecordId}", _mapper.Map<ReadTodoItemDto>(item));

            }
        }
        /// <summary>
        /// This method deletes to do item
        /// </summary>
        /// <param name="todoItemId">to-do item unique id</param>
        /// <returns>Returns success if item gets deleted successfully</returns>
        [HttpDelete("{todoItemId}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(StatusCodes.Status200OK)]



        public async Task<IActionResult> DeleteTodoItem(int todoItemId)
        {
            using (LogContext.PushProperty("Correlation Id", RequestInfo.GetCorrelationId(HttpContext.Request)))
            {
                if (todoItemId == 0)
                    return BadRequest(new { message = "To do item id cannot be zero" });

                var existingTodoItem = await _toDoService.GetToDoItem(todoItemId, int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
                if (existingTodoItem == null)
                {
                    return BadRequest("Resource not found with this unique id");
                }
                else
                {
                    await _toDoService.DeleteTodoItem(todoItemId);
                    Log.Information($"Record deleted successfully : {todoItemId}");
                    return Ok();
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
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(StatusCodes.Status200OK)]

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
                    return BadRequest("Resource not found with this unique id");
                }
                else
                {
                    var item = _mapper.Map<TblTodoItem>(todoItem);
                    await _toDoService.UpdateToDoItem(item, todoItemId);
                    Log.Information($"Record updated successfully. New record looks like: {MicrosoftJson.Serialize(item)}");
                    return Ok();
                }
            }

        }


        /// <summary>
        /// This method patches a record
        /// </summary>
        /// <param name="todoItemId">Unique to-do item id</param>
        /// <param name="todoItem">Patches info</param>
        /// <returns>Returns success if item gets patched successfulyy</returns>
        [HttpPatch("{todoItemId}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(StatusCodes.Status200OK)]

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
                    return BadRequest("Resource not found with this unique id");
                }
                else
                {
                    await _toDoService.UpdatePatchTodoItem(todoItem, todoItemId);
                    Log.Information($"Record patched successfully. Info was: {MicrosoftJson.Serialize(todoItem.Operations)}");
                    return Ok();
                }
            }
        }
    }
}

