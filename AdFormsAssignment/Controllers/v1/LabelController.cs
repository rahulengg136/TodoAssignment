using AdFormAssignment.DAL.Common;
using AdFormAssignment.DAL.Entities;
using AdFormsAssignment.BLL.Contracts;
using AdFormsAssignment.DTO.Common;
using AdFormsAssignment.DTO.GetDto;
using AdFormsAssignment.DTO.PostDto;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicrosoftJson = System.Text.Json.JsonSerializer;

namespace AdFormsAssignment.Controllers.v1
{
    /// <summary>
    /// Controller that Read, Create, Delete labels
    /// </summary>
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LabelController : ControllerBase
    {
        private readonly ILabelService _labelService;
        private readonly IMapper _mapper;
        /// <summary>
        /// Label controller
        /// </summary>
        /// <param name="labelService">Label service</param>
        /// <param name="mapper">Auto mapper</param>
        public LabelController(ILabelService labelService, IMapper mapper)
        {
            _labelService = labelService;
            _mapper = mapper;
        }
        /// <summary>
        /// This method returns list of labels based on the few search criteria and paging
        /// </summary>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize"> Page size- This will be the number of records accessible at one time</param>
        /// <param name="searchText">Any substring that may present in label name</param>
        /// <returns>Returns list of labels</returns>
        [HttpGet("Labels")]
        [ProducesResponseType(typeof(UnauthorizedInfo), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(IEnumerable<ReadLabelDto>), 200)]
        public async Task<IActionResult> GetAllLabels(int pageNumber, int pageSize, string searchText)
        {
            using (LogContext.PushProperty("Correlation Id", RequestInfo.GetCorrelationId(HttpContext.Request)))
            {
                var allLabels = await _labelService.GetAllLabels(pageNumber, pageSize, searchText);
                var tblLabels = allLabels.ToList();
                Log.Information($"Filtered labels: {MicrosoftJson.Serialize(tblLabels)}");
                if (!tblLabels.Any())
                {
                    return NoContent();
                }
                var filteredLabels = _mapper.Map<IEnumerable<ReadLabelDto>>(allLabels);
                return Ok(filteredLabels);
            }
        }
        /// <summary>
        /// This method gives details of a single label
        /// </summary>
        /// <param name="labelId">Unique label id</param>
        /// <returns>Returns details of single label</returns>
        [HttpGet("{labelId}")]
        [ProducesResponseType(typeof(UnauthorizedInfo), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(BadRequestInfo), 400)]
        [ProducesResponseType(typeof(ReadLabelDto), 200)]
        public async Task<IActionResult> GetSingleLabelInfo(int labelId)
        {
            using (LogContext.PushProperty("Correlation Id", RequestInfo.GetCorrelationId(HttpContext.Request)))
            {
                if (labelId == 0)
                {
                    return BadRequest(new { message = "Label id cannot be zero" });
                }
                var label = await _labelService.GetSingleLabelInfo(labelId);
                Log.Information($"Found label: {MicrosoftJson.Serialize(label)}");
                if (label != null)
                {
                    return Ok(_mapper.Map<ReadLabelDto>(label));
                }
                else
                {
                    return NoContent();
                }
            }
        }

        /// <summary>
        /// This method is to create a new label
        /// </summary>
        /// <param name="labelDto">Label info in json format should be posted</param>
        /// <returns>Return success in case label created successfully</returns>
        [HttpPost]
        [ProducesResponseType(typeof(UnauthorizedInfo), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ReadLabelDto), 201)]
        [ProducesResponseType(typeof(BadRequestInfo), 400)]
        public async Task<IActionResult> CreateLabel([FromBody] CreateLabelDto labelDto)
        {
            using (LogContext.PushProperty("Correlation Id", RequestInfo.GetCorrelationId(HttpContext.Request)))
            {
                if (string.IsNullOrEmpty(labelDto.LabelName))
                    return BadRequest(new { message = "Label name cannot be null or empty" });
                var label = _mapper.Map<TblLabel>(labelDto);
                int newRecordId = await _labelService.CreateLabel(label);
                Log.Information($"Record created successfully: {MicrosoftJson.Serialize(labelDto)}");
                return Created($"~/api/v1/label/{newRecordId}", _mapper.Map<ReadLabelDto>(label));
            }
        }

        /// <summary>
        /// This method is to delete a label
        /// </summary>
        /// <param name="labelId">Label id that needs to be deleted</param>
        /// <returns>Returns success if label gets deleted successfully</returns>
        [HttpDelete("{labelId}")]
        [ProducesResponseType(typeof(UnauthorizedInfo), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(TblLabel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestInfo), 400)]
        public async Task<IActionResult> DeleteLabel(int labelId)
        {
            using (LogContext.PushProperty("Correlation Id", RequestInfo.GetCorrelationId(HttpContext.Request)))
            {
                if (labelId == 0)
                    return BadRequest(new { message = "Label id cannot be zero" });
                var label = await _labelService.GetSingleLabelInfo(labelId);
                if (label != null)
                {
                    await _labelService.DeleteLabel(labelId);
                    Log.Information($"Label deleted successfully: {labelId}");
                    return Ok(label);
                }
                else
                {
                    return BadRequest(new { message = "No resource found with this label id" });
                }
            }
        }
    }
}
