using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApp.Cash;

public class CachedAttribute: Attribute, IAsyncActionFilter
{
    public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        throw new NotImplementedException();
    }
}