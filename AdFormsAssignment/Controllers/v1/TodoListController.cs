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
    /// This controller deals with CRUD operations of to-do lists
    /// </summary>
    [Authorize]
    [Route("api/v1/[controller]/")]
    [ApiController]
    public class TODOListController : ControllerBase
    {
        private readonly ITodoListService _toDoService;
        private readonly IMapper _mapper;
        /// <summary>
        /// To do list controller
        /// </summary>
        /// <param name="toDoService">to-do service instance</param>
        /// <param name="mapper">Automapper instance</param>
        public TODOListController(ITodoListService toDoService, IMapper mapper)
        {
            _toDoService = toDoService;
            _mapper = mapper;
        }

        /// <summary>
        /// This method filters  list of task-list 
        /// </summary>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="SearchText">Search text</param>
        /// <returns></returns>
        [HttpGet("allLists/{pageNumber}/{pageSize}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllTaskLists(int pageNumber, int pageSize, string SearchText)
        {
            using (LogContext.PushProperty("Correlation Id", RequestInfo.GetCorrelationId(HttpContext.Request)))
            {
                try
                {
                    Log.Information($"Entered GetAllTaskLists method with pageNumber {pageNumber}, pageSize {pageSize}, searchText {SearchText}");
                    var allTodoLists = await _toDoService.GetAllTodoLists(pageNumber, pageSize, SearchText, int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
                    Log.Information($"List of filtered to dos :{MicrosoftJson.Serialize(allTodoLists)}");
                    if (allTodoLists.Count() > 0)
                    {
                        return Ok(allTodoLists);
                    }
                    else
                    {
                        return NoContent();
                    }
                }
                catch (Exception exp)
                {
                    Log.Error($"Exception in GetAllTaskLists method :{MicrosoftJson.Serialize(exp.StackTrace)}");
                    return StatusCode(500, exp.ToString());
                }
            }
        }
        /// <summary>
        /// This method gives details of single to-do list
        /// </summary>
        /// <param name="todoListId">Todo list id</param>
        /// <returns></returns>
        [HttpGet("{todoListId}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTodoList(int todoListId)
        {
            using (LogContext.PushProperty("Correlation Id", RequestInfo.GetCorrelationId(HttpContext.Request)))
            {
                if (todoListId == 0)
                {
                    return BadRequest(new { message = "To do list id cannot be zero" });
                }
                try
                {
                    var list = await _toDoService.GetToDoList(todoListId, int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
                    Log.Information($"Found todo list. list id {todoListId} , {MicrosoftJson.Serialize(list)}");
                    if (list != null)
                    {
                        return Ok(list);
                    }
                    else {
                        return NoContent();
                    }
                }
                catch (Exception exp)
                {
                    Log.Error($"Exception in GetTodoList method :{MicrosoftJson.Serialize(exp.StackTrace)}");
                    return StatusCode(500, exp.ToString());
                }
            }
        }
        /// <summary>
        /// This method creates new to-do list
        /// </summary>
        /// <param name="todoList">list information</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateList([FromBody] TodoListDto todoList)
        {
            using (LogContext.PushProperty("Correlation Id", RequestInfo.GetCorrelationId(HttpContext.Request)))
            {
                // can do validation from fluent validation framework
                if (string.IsNullOrEmpty(todoList.ListName) || (todoList.UserId == 0))
                {
                    return BadRequest(new { message = "List name and user id is mandatory" });
                }
                try
                {
                    var list = _mapper.Map<tblTodoList>(todoList);
                    int newRecordId = await _toDoService.CreateToDoList(list);
                    Log.Information($"Newly created todo list {MicrosoftJson.Serialize(todoList)}");
                    return Created($"~/api/v1/TODO/{newRecordId}", list);
                }
                catch (Exception exp)
                {
                    Log.Error($"Exception in CreateList method :{MicrosoftJson.Serialize(exp.StackTrace)}");
                    return StatusCode(500, exp.ToString());
                }
            }
        }
        /// <summary>
        /// This method deletes a todo list
        /// </summary>
        /// <param name="todoListId">todo list id</param>
        /// <returns></returns>
        [HttpDelete("{todoListId}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> DeleteTodoList(int todoListId)
        {
            using (LogContext.PushProperty("Correlation Id", RequestInfo.GetCorrelationId(HttpContext.Request)))
            {
                if (todoListId == 0)
                    return BadRequest(new { message = "To do list id cannot be zero" });
                try
                {
                    var list = await _toDoService.GetToDoList(todoListId, int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));

                    if (list == null)
                    {
                        return BadRequest("Resource not found with this unique id");
                    }
                    else
                    {
                        await _toDoService.DeleteTodoList(todoListId);
                        Log.Information($"Delete to do list : {MicrosoftJson.Serialize(list)}");
                        return Ok();
                    }

                }
                catch (Exception exp)
                {
                    Log.Error($"Exception in DeleteTodoList method :{MicrosoftJson.Serialize(exp.StackTrace)}");
                    return StatusCode(500, exp.ToString());
                }
            }
        }
        /// <summary>
        /// This method updates to-do list
        /// </summary>
        /// <param name="todoListId">Todo list id</param>
        /// <param name="todoList">Updated details of list</param>
        /// <returns></returns>
        [HttpPut("{todoListId}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateTodoList(int todoListId, [FromBody] TodoListDto todoList)
        {
            using (LogContext.PushProperty("Correlation Id", RequestInfo.GetCorrelationId(HttpContext.Request)))
            {
                //considering list name is only mandatory
                if (todoListId == 0 || string.IsNullOrEmpty(todoList.ListName))
                {
                    return BadRequest(new { message = "to do list id cannot be zero. List name cannot be empty in body" });
                }
                try
                {
                    var existingList = await _toDoService.GetToDoList(todoListId, int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));

                    if (existingList == null)
                    {
                        return BadRequest("Resource not found with this unique id");
                    }
                    else
                    {
                        var list = _mapper.Map<tblTodoList>(todoList);
                        await _toDoService.UpdateToDoList(list, todoListId);
                        Log.Information($"Updated todo list {MicrosoftJson.Serialize(todoList)}");
                        return Ok();
                    }
                }
                catch (Exception exp)
                {
                    Log.Error($"Exception in UpdateTodoList method :{MicrosoftJson.Serialize(exp.StackTrace)}");
                    return StatusCode(500, exp.ToString());
                }
            }


        }
        /// <summary>
        /// This method patches a todo list
        /// </summary>
        /// <param name="todoListId">todo list id</param>
        /// <param name="todoList">Patches information</param>
        /// <returns></returns>
        [HttpPatch("{todoListId}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateTodoListPatch(int todoListId, [FromBody] JsonPatchDocument todoList)
        {
            using (LogContext.PushProperty("Correlation Id", RequestInfo.GetCorrelationId(HttpContext.Request)))
            {
                if (todoListId == 0 || todoList == null || todoList.Operations.Count == 0)
                {
                    return BadRequest(new { message = "To do list id cannot be zero. There should be atleast one operation" });
                }
                try
                {
                    var existingList = await _toDoService.GetToDoList(todoListId, int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));

                    if (existingList == null)
                    {
                        return BadRequest("Resource not found with this unique id");
                    }
                    else
                    {
                        await _toDoService.UpdatePatchTodoList(todoList, todoListId);
                        Log.Information($"Update todo list id {todoListId} with patch {MicrosoftJson.Serialize(todoList)}");
                        return Ok();
                    }
                }
                catch (Exception exp)
                {
                    Log.Error($"Exception in UpdateTodoList method :{MicrosoftJson.Serialize(exp.StackTrace)}");
                    return StatusCode(500, exp.ToString());
                }
            }

        }
    }
}
