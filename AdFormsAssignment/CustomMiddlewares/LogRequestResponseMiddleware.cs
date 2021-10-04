using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AdFormsAssignment.CustomMiddlewares
{
    /// <summary>
    /// Middleware that logs request and response
    /// </summary>
    public class LogRequestResponseMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LogRequestResponseMiddleware> _logger;
        private readonly Microsoft.IO.RecyclableMemoryStreamManager _recyclableMemoryStreamManager;

        /// <summary>
        /// Middleware that logs request and response
        /// </summary>
        public LogRequestResponseMiddleware(RequestDelegate next,
                                                ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory
                      .CreateLogger<LogRequestResponseMiddleware>();
            _recyclableMemoryStreamManager = new Microsoft.IO.RecyclableMemoryStreamManager();
        }

        /// <summary>
        /// Invoke method of the middleware
        /// </summary>
        /// <param name="context">Current Http Context</param>
        public async Task Invoke(HttpContext context)
        {
            await LogRequest(context);
            await _next(context);
            await LogResponse(context);

        }


        private async Task LogRequest(HttpContext context)
        {
            context.Request.EnableBuffering();

            context.Request.Headers.TryGetValue("x-correlation-id", out var traceValue);

            if (string.IsNullOrWhiteSpace(traceValue))
            {
                traceValue = Guid.NewGuid().ToString();
                context.Request.Headers.Add("x-correlation-id", traceValue);
            }

            await using var requestStream = _recyclableMemoryStreamManager.GetStream();
            await context.Request.Body.CopyToAsync(requestStream);
            _logger.LogInformation($"Http Request Information:{Environment.NewLine}" +
                                   $"Correlation Id:{traceValue} " +
                                   $"Schema:{context.Request.Scheme} " +
                                   $"Host: {context.Request.Host} " +
                                   $"Path: {context.Request.Path} " +
                                   $"QueryString: {context.Request.QueryString} " +
                                   $"Request Body: {ReadStreamInChunks(requestStream)}");
            context.Request.Body.Position = 0;
        }

        private static string ReadStreamInChunks(System.IO.Stream stream)
        {
            const int readChunkBufferLength = 4096;

            stream.Seek(0, System.IO.SeekOrigin.Begin);

            using var textWriter = new StringWriter();
            using var reader = new StreamReader(stream);

            var readChunk = new char[readChunkBufferLength];
            int readChunkLength;

            do
            {
                readChunkLength = reader.ReadBlock(readChunk,
                                                   0,
                                                   readChunkBufferLength);
                textWriter.Write(readChunk, 0, readChunkLength);
            } while (readChunkLength > 0);

            return textWriter.ToString();
        }

        private async Task LogResponse(HttpContext context)
        {
           

            var originalBodyStream = context.Response.Body;
           

            await using var responseBody = _recyclableMemoryStreamManager.GetStream();
            context.Response.Body = responseBody;

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            _logger.LogInformation($"Http Response Information:{Environment.NewLine}" +
                                   $"Schema:{context.Request.Scheme} " +
                                   $"Host: {context.Request.Host} " +
                                   $"Path: {context.Request.Path} " +
                                   $"QueryString: {context.Request.QueryString} " +
                                   $"Response Body: {text}");

            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
    /// <summary>
    /// Using the middleware
    /// </summary>
    public static class RequestResponseLoggingMiddlewareExtensions
    {
        /// <summary>
        /// Setting up logging-request-response middleware to the application
        /// </summary>
        /// <param name="builder"></param>
        public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LogRequestResponseMiddleware>();
        }
    }
}
