using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AE.Ships.Api.Filters;

public class LoggingActionFilter : IActionFilter
{
    private readonly ILogger<LoggingActionFilter> _logger;

    public LoggingActionFilter(ILogger<LoggingActionFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        var controllerName = context.Controller.GetType().Name;
        var actionName = context.ActionDescriptor.DisplayName;
        var parameters = context.ActionArguments;

        _logger.LogInformation("Executing {ControllerName}.{ActionName} with parameters: {@Parameters}", 
            controllerName, actionName, parameters);
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        var controllerName = context.Controller.GetType().Name;
        var actionName = context.ActionDescriptor.DisplayName;
        var result = context.Result;

        if (result is ObjectResult objectResult)
        {
            _logger.LogInformation("Completed {ControllerName}.{ActionName} with status {StatusCode} and result: {@Result}", 
                controllerName, actionName, objectResult.StatusCode, objectResult.Value);
        }
        else
        {
            _logger.LogInformation("Completed {ControllerName}.{ActionName} with result: {Result}", 
                controllerName, actionName, result?.GetType().Name);
        }
    }
}

