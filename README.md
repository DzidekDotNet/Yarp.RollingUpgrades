
# Yarp Simple Rolling upgrades and AB tests

Yarp Rolling Upgrades is an extendable and easy-to-use extension for Yarp Reverse Proxy


## Basic usage

Install Yarp.ReverseProxy with NuGet

Add to Program.cs Yarp and load config from appsettings file. [Yarp documentation](https://microsoft.github.io/reverse-proxy/articles/getting-started.html)
```cs
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
...
app.MapReverseProxy();
```

Install Dzidek.Net.Yarp.RollingUpgrades with NuGet

Replace 
```cs
app.MapReverseProxy();
```

with
```cs
app.MapReverseProxy(proxyPipeline => { proxyPipeline.UseRollingUpgrades(new RollingUpgradesRules()); });
```

Create class RollingUpgradesRules with rolling upgrades configuration. This configuration can be static or dynamic from a file or database or whenever you want
```cs
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
```cs
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
```cs
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
```cs
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
## Roadmap

- Add more HTTP request props allowed in rules


## Versioning policy
The project major version will be the same as the DotNetCore version

## Nuget
[Dzidek.Net.Yarp.RollingUpgrades](https://www.nuget.org/packages/Dzidek.Net.Yarp.RollingUpgrades)


## Authors

- [@DzidekDotNet](https://www.github.com/DzidekDotNet)


## License

[MIT](https://github.com/DzidekDotNet/Yarp.RollingUpgrades/blob/main/LICENSE)

