using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace WebsiteManagement.Api.Filters
{
    public class LoggingFilter : IActionFilter
    {
        private readonly ILogger<LoggingFilter> _logger;

        private Stopwatch _stopwatch;
        private string _requestId;

        public LoggingFilter(ILogger<LoggingFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _requestId = Guid.NewGuid().ToString();
            _stopwatch = Stopwatch.StartNew();

            _logger.LogInformation($"[{_requestId}]: Calling endpoint {context.HttpContext.Request.Method} | {context.HttpContext.Request.Path} ");
        }


        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation($"[{_requestId}]: Request finished for {_stopwatch.ElapsedMilliseconds}ms.");
        }
    }
}
