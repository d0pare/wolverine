using IntegrationTests;
using Marten;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Oakton;
using Oakton.Resources;
using Wolverine;
using Wolverine.EntityFrameworkCore;
using Wolverine.Http;
using Wolverine.Marten;
using WolverineWebApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContextWithWolverineIntegration<ItemsDbContext>(
    x => x.UseNpgsql(Servers.PostgresConnectionString));


builder.Services.AddMarten(opts =>
{
    opts.Connection(Servers.PostgresConnectionString);
    opts.DatabaseSchemaName = "http";
}).IntegrateWithWolverine();



builder.Services.AddResourceSetupOnStartup();

builder.Services.AddSingleton<Recorder>();

// Need this.
builder.Host.UseWolverine(opts =>
{
    // Set up Entity Framework Core as the support
    // for Wolverine's transactional middleware
    opts.UseEntityFrameworkCoreTransactions();
    
    opts.Policies.AutoApplyTransactions();
    
    opts.OptimizeArtifactWorkflow();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.MapGet("/orders/{orderId}", [Authorize] Results<BadRequest, Ok<Order>> (int orderId) 
    => orderId > 999 ? TypedResults.BadRequest() : TypedResults.Ok(new Order(orderId)));

app.MapPost("/orders", Results<BadRequest, Ok<Order>> (CreateOrder command) 
    => command.OrderId > 999 ? TypedResults.BadRequest() : TypedResults.Ok(new Order(command.OrderId)));

app.MapWolverineEndpoints();

await app.RunOaktonCommands(args);