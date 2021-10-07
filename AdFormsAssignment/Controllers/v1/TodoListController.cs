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
    /// This controller deals with CRUD operations of to-do lists
    /// </summary>
    [Authorize]
    [Route("api/v1/[controller]/")]
    [ApiController]
    public class ToDoListController : ControllerBase
    {
        private readonly ITodoListService _toDoService;
        private readonly IMapper _mapper;
        /// <summary>
        /// To do list controller
        /// </summary>
        /// <param name="toDoService">to-do service instance</param>
        /// <param name="mapper">AutoMapper instance</param>
        public ToDoListController(ITodoListService toDoService, IMapper mapper)
        {
            _toDoService = toDoService;
            _mapper = mapper;
        }

        /// <summary>
        /// This method filters  list of task-list 
        /// </summary>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="searchText">Search text</param>
        /// <returns>Returns list of task lists</returns>
        [HttpGet("Lists")]
        [ProducesResponseType(typeof(UnauthorizedInfo), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(IEnumerable<ReadTodoListDto>), 200)]
        public async Task<IActionResult> GetAllTaskLists(int pageNumber, int pageSize, string searchText)
        {
            using (LogContext.PushProperty("Correlation Id", RequestInfo.GetCorrelationId(HttpContext.Request)))
            {
                Log.Information($"Entered GetAllTaskLists method with pageNumber {pageNumber}, pageSize {pageSize}, searchText {searchText}");
                var allTodoLists = await _toDoService.GetAllTodoLists(pageNumber, pageSize, searchText, int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
                var todoListDetails = allTodoLists.ToList();
                Log.Information($"List of filtered to dos :{MicrosoftJson.Serialize(todoListDetails)}");
                if (todoListDetails.Any())
                {
                    return Ok(_mapper.Map<IEnumerable<ReadTodoListDto>>(allTodoLists));
                }
                else
                {
                    return NoContent();
                }
            }
        }
        /// <summary>
        /// This method gives details of single to-do list
        /// </summary>
        /// <param name="todoListId">To-do list id</param>
        /// <returns>Returns detail of single to-do item</returns>
        [HttpGet("{todoListId}")]
        [ProducesResponseType(typeof(UnauthorizedInfo), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ReadTodoListDto), 200)]
        [ProducesResponseType(typeof(BadRequestInfo), 400)]
        public async Task<IActionResult> GetTodoList(int todoListId)
        {
            using (LogContext.PushProperty("Correlation Id", RequestInfo.GetCorrelationId(HttpContext.Request)))
            {
                if (todoListId == 0)
                {
                    return BadRequest(new { message = "To do list id cannot be zero" });
                }
                var list = await _toDoService.GetToDoList(todoListId, int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
                Log.Information($"Found todo list. list id {todoListId} , {MicrosoftJson.Serialize(list)}");
                if (list != null)
                {
                    return Ok(_mapper.Map<ReadTodoListDto>(list));
                }
                else
                {
                    return NoContent();
                }
            }
        }
        /// <summary>
        /// This method creates new to-do list
        /// </summary>
        /// <param name="todoList">list information</param>
        /// <returns>Returns success if to-do list gets created successfully</returns>
        [HttpPost]
        [ProducesResponseType(typeof(UnauthorizedInfo), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ReadTodoListDto), 201)]
        [ProducesResponseType(typeof(BadRequestInfo), 400)]
        public async Task<IActionResult> CreateList([FromBody] CreateTodoListDto todoList)
        {
            using (LogContext.PushProperty("Correlation Id", RequestInfo.GetCorrelationId(HttpContext.Request)))
            {
                // can do validation from fluent validation framework
                if (string.IsNullOrEmpty(todoList.ListName))
                {
                    return BadRequest(new { message = "List name and user id is mandatory" });
                }
                var list = _mapper.Map<TblTodoListExtension>(todoList);
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                int newRecordId = await _toDoService.CreateToDoList(list, userId);
                Log.Information($"Newly created todo list {MicrosoftJson.Serialize(todoList)}");
                var newlyCreatedRecord = await _toDoService.GetToDoList(newRecordId, int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
                return Created($"~/api/v1/TODO/{newRecordId}", newlyCreatedRecord);
            }
        }
        /// <summary>
        /// This method deletes a to-do list
        /// </summary>
        /// <param name="todoListId">to-do list id</param>
        /// <returns>Returns success if to-do list gets deleted successfully</returns>
        [HttpDelete("{todoListId}")]
        [ProducesResponseType(typeof(UnauthorizedInfo), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(TodoListDetail), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestInfo), 400)]

        public async Task<IActionResult> DeleteTodoList(int todoListId)
        {
            using (LogContext.PushProperty("Correlation Id", RequestInfo.GetCorrelationId(HttpContext.Request)))
            {
                if (todoListId == 0)
                    return BadRequest(new { message = "To do list id cannot be zero" });
                var list = await _toDoService.GetToDoList(todoListId, int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
                if (list == null)
                {
                    return BadRequest(new { message = "Resource not found with this unique id" });
                }
                else
                {
                    await _toDoService.DeleteTodoList(todoListId);
                    Log.Information($"Delete to do list : {MicrosoftJson.Serialize(list)}");
                    return Ok(list);
                }
            }
        }
        /// <summary>
        /// This method updates to-do list
        /// </summary>
        /// <param name="todoListId">To-do list id</param>
        /// <param name="todoList">Updated details of list</param>
        /// <returns>Returns success if to-do list gets updated successfully</returns>
        [HttpPut("{todoListId}")]
        [ProducesResponseType(typeof(UnauthorizedInfo), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(TodoListDetail), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestInfo), 400)]
        public async Task<IActionResult> UpdateTodoList(int todoListId, [FromBody] CreateTodoListDto todoList)
        {
            using (LogContext.PushProperty("Correlation Id", RequestInfo.GetCorrelationId(HttpContext.Request)))
            {
                //considering list name is only mandatory
                if (todoListId == 0 || string.IsNullOrEmpty(todoList.ListName))
                {
                    return BadRequest(new { message = "to do list id cannot be zero. List name cannot be empty in body" });
                }
                var existingList = await _toDoService.GetToDoList(todoListId, int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
                if (existingList == null)
                {
                    return BadRequest(new { Message = "Resource not found with this unique id" });
                }
                else
                {
                    var list = _mapper.Map<TblTodoListExtension>(todoList);
                    var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                    await _toDoService.UpdateToDoList(list, todoListId, userId);
                    Log.Information($"Updated todo list {MicrosoftJson.Serialize(todoList)}");
                    return Ok(await _toDoService.GetToDoList(todoListId, int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value)));
                }
            }
        }

        /// <summary>
        /// This method patches a to-do list
        /// </summary>
        /// <param name="todoListId">to-do list id</param>
        /// <param name="todoList">Patches information</param>
        /// <returns>Returns success if to-do list gets patched successfully</returns>
        [HttpPatch("{todoListId}")]
        [ProducesResponseType(typeof(UnauthorizedInfo), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(TodoListDetail), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestInfo), 400)]
        public async Task<IActionResult> UpdateTodoListPatch(int todoListId, [FromBody] JsonPatchDocument todoList)
        {
            using (LogContext.PushProperty("Correlation Id", RequestInfo.GetCorrelationId(HttpContext.Request)))
            {
                if (todoListId == 0 || todoList == null || todoList.Operations.Count == 0)
                {
                    return BadRequest(new { message = "To do list id cannot be zero. There should be at least one operation" });
                }

                var existingList = await _toDoService.GetToDoList(todoListId, int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
                if (existingList == null)
                {
                    return BadRequest("Resource not found with this unique id");
                }
                else
                {
                    await _toDoService.UpdatePatchTodoList(todoList, todoListId);
                    Log.Information($"Update todo list id {todoListId} with patch {MicrosoftJson.Serialize(todoList)}");
                    return Ok(await _toDoService.GetToDoList(todoListId, int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value)));
                }
            }
        }
    }
}
