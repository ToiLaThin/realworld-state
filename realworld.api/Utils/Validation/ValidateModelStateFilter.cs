using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

/// <summary>
/// This filter is registered globally in the app startup
/// </summary>
public class ValidateModelStateFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var result = new ContentResult();
            var errors = new Dictionary<string, string[]>();

            foreach (var valuePair in context.ModelState) {
                errors.Add(
                    valuePair.Key, 
                    valuePair.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );
            }

            var content = JsonSerializer.Serialize(new { errors });
            result.Content = content;
            result.ContentType = "application/json";
            
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;//422
            context.Result = result;
           
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}