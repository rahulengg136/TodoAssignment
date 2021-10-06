using AdFormsAssignment.BLL.Contracts;
using AdFormsAssignment.GraphQL;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace AdFormsAssignment.Controllers
{

    public class GraphQLQuery
    {
        public string OperationName { get; set; }
        public string NamedQuery { get; set; }
        public string Query { get; set; }
        public JObject Variables { get; set; }
    }



    [Route("graphql")]
    [ApiController]
    public class GraphQLController : Controller
    {
        private readonly ILabelService _labelService;
        private readonly ITodoItemService _todoItemService;
        private readonly ITodoListService _todoListService;

        public GraphQLController(ILabelService labelService, ITodoItemService todoItemService, ITodoListService todoListService)
        {
            _labelService = labelService;
            _todoItemService = todoItemService;
            _todoListService = todoListService;

        }

        [Authorize]
        public async Task<IActionResult> Post([FromBody] GraphQLQuery query)
        {
            var inputs = query.Variables.ToInputs();

            var schema = new Schema
            {
                Query = new Queries(_labelService, _todoItemService, _todoListService)
            };

            var result = await new DocumentExecuter().ExecuteAsync(_ =>
            {
                _.Schema = schema;
                _.Query = query.Query;
                _.OperationName = query.OperationName;
                _.Inputs = inputs;
            });

            if (result.Errors?.Count > 0)
            {
                return BadRequest();
            }

            return Ok(result);
        }
    }
}
