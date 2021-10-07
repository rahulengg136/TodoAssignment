using AdFormsAssignment.DTO.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace AdFormsAssignment.Configuration
{
    /// <summary>
    /// Class to add response codes at global level 
    /// </summary>
    public class ProduceResponseTypeModelProvider : IApplicationModelProvider
    {
        /// <summary>
        /// For sequence of execution
        /// </summary>
        public int Order => 0;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public void OnProvidersExecuted(ApplicationModelProviderContext context)
        {
            // Method intentionally left empty.
        }
        /// <summary>
        /// Method to add status code for all methods in application
        /// </summary>
        /// <param name="context"></param>
        public void OnProvidersExecuting(ApplicationModelProviderContext context)
        {
            foreach (ControllerModel controller in context.Result.Controllers)
            {
                foreach (ActionModel action in controller.Actions)
                {
                    action.Filters.Add(new ProducesResponseTypeAttribute(typeof(UnauthorizedInfo), 401));
                }
            }
        }
    }
}
