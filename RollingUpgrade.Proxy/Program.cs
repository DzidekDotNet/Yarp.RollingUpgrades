using Dzidek.Net.Yarp.RollingUpgrades;
using Dzidek.Net.Yarp.RollingUpgrades.Extensions.Api;
using Dzidek.Net.Yarp.RollingUpgrades.Rules;
using RollingUpgrade.Proxy;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
// Example of Api configuration
builder.Services.UseRollingUpgradesApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
// Basic example
app.MapReverseProxy(proxyPipeline => { proxyPipeline.UseRollingUpgrades(new RollingUpgradesRules()); });
// Example of Api configuration -> for Api Configuration line below should be uncomment and line above should be comment
//app.MapReverseProxy(proxyPipeline => { proxyPipeline.UseRollingUpgrades(); });
app.Run();