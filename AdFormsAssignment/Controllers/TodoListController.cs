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
    public class TODOListController : ControllerBase
    {
        private readonly ITodoListService _toDoService;
        private readonly IMapper _mapper;
        public TODOListController(ITodoListService toDoService, IMapper mapper)
        {
            _toDoService = toDoService;
            _mapper = mapper;
        }

        [HttpGet("allLists/{pageNumber}/{pageSize}")]
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
        [HttpGet("{todoListId}")]
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
                    else { return NoContent(); }
                }
                catch (Exception exp)
                {
                    Log.Error($"Exception in GetTodoList method :{MicrosoftJson.Serialize(exp.StackTrace)}");
                    return StatusCode(500, exp.ToString());
                }
            }
        }

        [HttpPost]
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

        [HttpDelete("{todoListId}")]
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
        [HttpPut("{todoListId}")]
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
                    var list = _mapper.Map<tblTodoList>(todoList);
                    await _toDoService.UpdateToDoList(list, todoListId);
                    Log.Information($"Updated todo list {MicrosoftJson.Serialize(todoList)}");
                    return Ok();
                }
                catch (Exception exp)
                {
                    Log.Error($"Exception in UpdateTodoList method :{MicrosoftJson.Serialize(exp.StackTrace)}");
                    return StatusCode(500, exp.ToString());
                }
            }


        }
        [HttpPatch("{todoListId}")]
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
                    await _toDoService.UpdatePatchTodoList(todoList, todoListId);
                    Log.Information($"Update todo list id {todoListId} with patch {MicrosoftJson.Serialize(todoList)}");
                    return Ok();
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
