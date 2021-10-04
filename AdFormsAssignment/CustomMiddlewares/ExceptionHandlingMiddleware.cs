using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Microsoft.Extensions.Primitives;

namespace AdFormsAssignment.CustomMiddlewares
{
    /// <summary>
    /// Exception handling middleware
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        /// <summary>
        /// Exception handling
        /// </summary>
        /// <param name="next"></param>
        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Invoke method of middleware
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exp)
            {
                context.Request.Headers.TryGetValue("X-Correlation-ID", out StringValues correlationId);
                Log.Error($"Exception in application. Correlation Id {correlationId} Message {exp.Message} Stacktrace {exp.StackTrace}");
                throw;
            }
        }
    }
}

