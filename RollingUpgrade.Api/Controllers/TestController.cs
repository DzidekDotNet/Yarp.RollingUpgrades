using Microsoft.AspNetCore.Mvc;

namespace RollingUpgrade.Api.Controllers;

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