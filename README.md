
# Simple rolling upgrades and AB tests with scheduler

Yarp Rolling Upgrades is an extendable and easy-to-use extension for scheduled upgrades or AB test


## Basic usage

Install Yarp.ReverseProxy with NuGet

Add to Program.cs Yarp and load config from appsettings file. [Yarp documentation](https://microsoft.github.io/reverse-proxy/articles/getting-started.html)
```csharp
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
...
app.MapReverseProxy();
```

Install Dzidek.Net.Yarp.RollingUpgrades with NuGet

Replace 
```csharp
app.MapReverseProxy();
```

with
```csharp
app.MapReverseProxy(proxyPipeline => { proxyPipeline.UseRollingUpgrades(new RollingUpgradesRules()); });
```

Create class RollingUpgradesRules with rolling upgrades configuration. This configuration can be static or dynamic from a file or database or whenever you want
```csharp
internal sealed class RollingUpgradesRules : IRollingUpgradesRulesQuery
{
    public IEnumerable<RollingUpgradesRule> GetRules()
    {
        return new List<RollingUpgradesRule>()
        {
            new RollingUpgradesRule("ApiRoute", "Api2", new HeaderRule("TenantId", "1"))
        };
    }
}
```
## Defined rules
- BasicRule
  - AllRule - all request will be directed to defined cluster
  ```csharp
  new AllRule()
  ```
  - CookieRule - request with contains cookie with defined value will be directed to defined cluster
  ```csharp
  new CookieRule("CookieName", "CookieValue");
  ```
  - HeaderRule - request with contains header with defined value will be directed to defined cluster
  ```csharp
  new HeaderRule("HeaderName", "HeaderValue");
  ```
  - UrlContainsRule - request with contains in url defined value will be directed to defined cluster
  ```csharp
  new UrlContainsRule("UrlValue");
  ```
- ScheduledRules - rules with allow to plan and defer changing about changing cluster. DateFrom, DateTo describe when rule is valid 
    - AllRule - all request will be directed to defined cluster
  ```csharp
  new AllRule(DateFrom, DateTo)
  ```
    - CookieRule - request with contains cookie with defined value will be directed to defined cluster
  ```csharp
  new CookieRule("CookieName", "CookieValue", DateFrom, DateTo);
  ```
    - HeaderRule - request with contains header with defined value will be directed to defined cluster
  ```csharp
  new HeaderRule("HeaderName", "HeaderValue", DateFrom, DateTo);
  ```
    - UrlContainsRule - request with contains in url defined value will be directed to defined cluster
  ```csharp
  new UrlContainsRule("UrlValue", DateFrom, DateTo);
  ```
    
## Example and testing environment

In the repository, there is a working docker-compose environment with a proxy and 2 clusters of API, with an easy way to upgrade API on the fly.

Project RollingUpgrade.Proxy contains the implementation of Yarp and Yarp.RollingUpgrades (configured to send a request to Api2 cluster when HTTP request header contains tenantId header with value 1). The proxy is running on port: 8000

Project RollingUpgrade.Api contains TestController with the get method which returns an integer

Project docker-compose can instantiate one proxy and 2 API instances in different clusters

To test you should download this repository and run:
```
docker-compose up -d --build
```
Next, you can check what you get calling API (it should return 1 in both cases)
```
curl http://localhost:8000/Test -H "TenantId: 1"
curl http://localhost:8000/Test -H "TenantId: 2"
```
Next, you should change the return value from 1 to whatever you want in the TestController file in RollingUpgrade.Api project
```csharp
[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    [HttpGet]
    public Task<int> Get()
    {
        return Task.FromResult(2);
    }
}
```
Next, you should upgrade only Api2 instance 
```
docker-compose up -d --build rollingupgrade.api2
```
And check now what you get for different TenantId headers (for tenantId 1 you should get 2 instead of 1)
```
curl http://localhost:8000/Test -H "TenantId: 1"
curl http://localhost:8000/Test -H "TenantId: 2"
```
## Create your own rolling upgrade rule

you have to implement the abstract class RuleBase with the method IsValid
```csharp
public class CustomRule : RuleBase
{
    private readonly string _headerName1;
    private readonly string _headerValue1;
    private readonly string _headerName2;
    private readonly string _headerValue2;

    public CustomRule(string headerName1, string headerValue1, string headerName2, string headerValue2)
    {
        _headerName1 = headerName1;
        _headerValue1 = headerValue1;
        _headerName2 = headerName2;
        _headerValue2 = headerValue2;
    }

    public override bool IsValid(IClusterChooserHttpContext httpContext)
    {
        return httpContext.Headers.ContainsKey(_headerName1) && httpContext.Headers[_headerName1].Contains(_headerValue1)
            && httpContext.Headers.ContainsKey(_headerName2) && httpContext.Headers[_headerName2].Contains(_headerValue2);
    }
}
```

and yours it in the same way as predefined RuleBase
```csharp
internal sealed class RollingUpgradesRules : IRollingUpgradesRulesQuery
{
    public IEnumerable<RollingUpgradesRule> GetRules()
    {
        return new List<RollingUpgradesRule>()
        {
            new RollingUpgradesRule("ApiRoute", "Api2", new CustomRule("TenantId", "1", "OtherHeader", "1"))
        };
    }
}
```
and that's it :)

You can check it by doing the same steps as in 'Example and testing environment' but add an extra header in the curl request
```
curl http://localhost:8000/Test -H "TenantId: 1" -H "OtherHeader: 1"
```
## HttpContext properties allowed in rules
```csharp
public interface IClusterChooserHttpContext
{
    string Url { get; }
    string Method { get; }
    string? ContentType { get; }
    IDictionary<string, StringValues> Headers { get; }
    IEnumerable<KeyValuePair<string, string>> Cookies { get; }
    IEnumerable<KeyValuePair<string, StringValues>> Form { get; }
    IEnumerable<KeyValuePair<string, StringValues>> Query { get; }
}
```

## Changelog
- 7.0.3 and 6.0.3
  - Add scheduler
  - Changing cluster only when at least one destination is healthy (if configured health check in yarp)
  - Extended allowed properties from HttpContext in rules

## Roadmap

- Add extension allow to configure rolling upgrades from API (API request allowed from localhost)
- Load last configuration from saved file in API extension

## Versioning policy
The project major version will be the same as the DotNetCore version

## Nuget
[Dzidek.Net.Yarp.RollingUpgrades](https://www.nuget.org/packages/Dzidek.Net.Yarp.RollingUpgrades)


## Authors

- [@DzidekDotNet](https://www.github.com/DzidekDotNet)


## License

[MIT](https://github.com/DzidekDotNet/Yarp.RollingUpgrades/blob/main/LICENSE)

