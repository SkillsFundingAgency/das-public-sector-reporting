using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SFA.DAS.PSRService.Web.Middleware;

namespace SFA.DAS.PSRService.Web.Middleware
{
    public class ErrorLoggingMiddleware(RequestDelegate next, ILogger<ErrorLoggingMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                //Invoke the next call in the middleware tree
                await next(context);
            }
            catch (Exception ex)
            {
                //If any other middleware has thrown an error and it hasnt been dealt with we will end up here
                try
                {
                    logger.LogError(ex, "Unhandled Exception in {RequestPath}.", context.Request.Path);
                }
                //if logging doesn't work we have a problem! but we still need to hande it, this will allow us to see both exceptions
                catch (Exception loggingEx)
                {
                    var exceptionList = new List<Exception>
                    {
                        ex,
                        loggingEx
                    };

                    var aggregatedException = new AggregateException(exceptionList.AsEnumerable());
                    
                    throw aggregatedException;
                }
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