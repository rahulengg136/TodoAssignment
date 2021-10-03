using AdFormAssignment.DAL.Common;
using AdFormAssignment.DAL.Entities;
using AdFormsAssignment.BLL.Contracts;
using AdFormsAssignment.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Context;
using System;
using System.Linq;
using System.Threading.Tasks;
using MicrosoftJson = System.Text.Json.JsonSerializer;

namespace AdFormsAssignment.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LabelController : ControllerBase
    {
        private readonly ILabelService _labelService;
        private readonly IMapper _mapper;

        public LabelController(ILabelService labelService, IMapper mapper)
        {
            _labelService = labelService;
            _mapper = mapper;
        }

        [HttpGet("allLabels/{pageNumber}/{pageSize}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllLabels(int pageNumber, int pageSize, string SearchText)
        {
            using (LogContext.PushProperty("Correlation Id", RequestInfo.GetCorrelationId(HttpContext.Request)))
            {
                try
                {
                    var allLables = await _labelService.GetAllLabels(pageNumber, pageSize, SearchText);
                    Log.Information($"Filtered labels: {MicrosoftJson.Serialize(allLables)}");
                    if (allLables.Count() == 0)
                        return NoContent();
                    return Ok(allLables);


                }
                catch (Exception exp)
                {
                    Log.Error($"Exception in method :{MicrosoftJson.Serialize(exp.StackTrace)}");
                    return StatusCode(500, exp.ToString());
                }
            }
        }
        [HttpGet("{labelId}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSingleLabelInfo(int labelId)
        {
            using (LogContext.PushProperty("Correlation Id", RequestInfo.GetCorrelationId(HttpContext.Request)))
            {
                if (labelId == 0)
                    return BadRequest(new { message = "Label id cannot be zero" });
                try
                {
                    var label = await _labelService.GetSingleLabelInfo(labelId);
                    Log.Information($"Found label: {MicrosoftJson.Serialize(label)}");
                    if (label != null)
                    {
                        return Ok(label);
                    }
                    else
                    {
                        return BadRequest("No resource found with this label id");
                    }

                }
                catch (Exception exp)
                {
                    Log.Error($"Exception in method :{MicrosoftJson.Serialize(exp.StackTrace)}");
                    return StatusCode(500, exp.ToString());
                }
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateLabel([FromBody] LabelDto labelDto)
        {
            using (LogContext.PushProperty("Correlation Id", RequestInfo.GetCorrelationId(HttpContext.Request)))
            {
                if (string.IsNullOrEmpty(labelDto.LabelName))
                    return BadRequest(new { message = "Label name cannot be null or empty" });

                try
                {
                    var label = _mapper.Map<tblLabel>(labelDto);
                    int newRecordId = await _labelService.CreateLabel(label);
                    Log.Information($"Record created successfully: {MicrosoftJson.Serialize(labelDto)}");
                    return Created($"~/api/v1/label/{newRecordId}", labelDto);
                }
                catch (Exception exp)
                {
                    Log.Error($"Exception in method :{MicrosoftJson.Serialize(exp.StackTrace)}");
                    return StatusCode(500, exp.ToString());
                }
            }
        }

        [HttpDelete("{labelId}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteTodoLabel(int labelId)
        {
            using (LogContext.PushProperty("Correlation Id", RequestInfo.GetCorrelationId(HttpContext.Request)))
            {
                if (labelId == 0)
                    return BadRequest(new { message = "Label id cannot be zero" });
                try
                {
                    var label = await _labelService.GetSingleLabelInfo(labelId);
                    if (label != null)
                    {
                        await _labelService.DeleteLabel(labelId);
                        Log.Information($"Label deleted successfully: {labelId}");
                        return Ok();
                    }
                    else
                    {
                        return BadRequest("No resource found with this label id");
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
