using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Dzidek.Net.Yarp.RollingUpgrades.Extensions.Api;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class ApiKeyAttribute : Attribute, IAuthorizationFilter
{
    private const string API_KEY_HEADER_NAME = "X-API-Key";

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var submittedApiKey = GetSubmittedApiKey(context.HttpContext);

        var apiKey = GetApiKey(context.HttpContext);

        if (apiKey != submittedApiKey)
        {
            context.Result = new UnauthorizedResult();
        }
    }

    private static string GetSubmittedApiKey(HttpContext context)
    {
        return context.Request.Headers[API_KEY_HEADER_NAME];
    }

    private static string GetApiKey(HttpContext context)
    {
        var configuration = context.RequestServices.GetRequiredService<IApiKeyConfiguration>();

        return configuration.ApiKey;
    }
}