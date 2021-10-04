using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CorrelationId.CorrelationIdWork
{
    /// <summary>
    /// Middle ware that appends correlation id to a request header
    /// </summary>
    public class CorrelationMiddleware
    {
        private const string CorrelationIdHeaderKey = "X-Correlation-ID";
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        /// <summary>
        /// Middle ware that appends correlation id to a request header
        /// </summary>
        /// <param name="next"></param>
        /// <param name="loggerFactory"></param>
        public CorrelationMiddleware(RequestDelegate next,
        ILoggerFactory loggerFactory)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = loggerFactory.CreateLogger<CorrelationMiddleware>();
        }
        /// <summary>
        /// Invoke method of correlation id middleware
        /// </summary>
        /// <param name="httpContext"></param>
        public async Task Invoke(HttpContext httpContext)
        {
            string correlationId = null;
            httpContext.Request.Headers.Add("Accept",
               "application/xml");
            if (httpContext.Request.Headers.TryGetValue(CorrelationIdHeaderKey, out StringValues correlationIds))
            {
                correlationId = correlationIds.FirstOrDefault(k =>
                k.Equals(CorrelationIdHeaderKey));
                _logger.LogInformation($"CorrelationId from Request Header:{ correlationId}");
            }
            else
            {
                correlationId = Guid.NewGuid().ToString();
                httpContext.Request.Headers.Add(CorrelationIdHeaderKey,
                correlationId);
                _logger.LogInformation($"Generated CorrelationId:{ correlationId}");


            }
            httpContext.Response.OnStarting(() =>
            {
                if (!httpContext.Response.Headers.TryGetValue(CorrelationIdHeaderKey, out correlationIds))
                    httpContext.Response.Headers.Add(CorrelationIdHeaderKey, correlationId);
                return Task.CompletedTask;
            });
            await _next.Invoke(httpContext);
        }
    }
}
