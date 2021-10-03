using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace AdFormsAssignment
{
    public class AddParametersToSwagger : IOperationFilter
    {
        //headers in swagger
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null) operation.Parameters = new List<OpenApiParameter>();


            if (context.ApiDescription.ActionDescriptor is ControllerActionDescriptor)
            {
                operation.Parameters.Add(new OpenApiParameter()
                {
                    Name = "Accept",
                    In = ParameterLocation.Header,
                    Description = "",
                    Required = false,
                    
                });
             

            }
        }
    }
}
