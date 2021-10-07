using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace AdFormsAssignment.Configuration
{
    /// <summary>
    /// Class that add parameters to swagger request
    /// </summary>
    public class AddParametersToSwagger : IOperationFilter
    {
        /// <summary>
        /// Written Apply method
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= new List<OpenApiParameter>();
            if (context.ApiDescription.ActionDescriptor is ControllerActionDescriptor)
            {
                operation.Parameters.Add(new OpenApiParameter()
                {
                    Name = "x-correlation-id",
                    In = ParameterLocation.Header,
                    Description = "",
                    Required = false
                });
                operation.Parameters.Add(new OpenApiParameter()
                {
                    Name = "Accept",
                    In = ParameterLocation.Header,
                    Description = "",
                    Required = false
                });
                operation.Parameters.Add(new OpenApiParameter()
                {
                    Name = "content-location",
                    In = ParameterLocation.Header,
                    Description = "",
                    Required = false
                });
            }
        }
    }
}
