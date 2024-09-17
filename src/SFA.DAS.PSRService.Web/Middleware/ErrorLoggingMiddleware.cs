using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SFA.DAS.PSRService.Web.Middleware;

namespace SFA.DAS.PSRService.Web.Middleware
{
    public class ErrorLoggingMiddleware
    {
        private readonly ILogger<ErrorLoggingMiddleware> _logger;
        private readonly RequestDelegate _next;

        public ErrorLoggingMiddleware(RequestDelegate next, ILogger<ErrorLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                //Invoke the next call in the middleware tree
                await _next(context);
            }
            catch (Exception ex)
            {
                //If any other middleware has thrown an error and it hasnt been dealt with we will end up here
                try
                {
                    //log the error.
                    _logger.LogError($"Unhandled Exeption in {context.Request.Path} raised : {ex.Message} : Stack Trace : {ex.StackTrace}");
                }
                //if logging doesnt work we have a problem! but we still need to hande it, this will allow us to see both exceptions
                catch (Exception loggingEx)
                {
                    IList<Exception> exceptionList = new List<Exception>();

                    exceptionList.Add(ex);
                    exceptionList.Add(loggingEx);
                    var aggregatedException = new AggregateException(exceptionList.AsEnumerable());
                    
                    throw aggregatedException;
                }
                //throw the error to allow the default unhandled exception behaviour
                throw;
            }
        }
    }
}

namespace Microsoft.AspNetCore.Builder
{
    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ErrorLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorLoggingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorLoggingMiddleware>();
        }
    }
}