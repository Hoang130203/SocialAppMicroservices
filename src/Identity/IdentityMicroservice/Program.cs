using Serilog;
using Serilog.Events;
using MongoDB.Driver;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Middleware;
using IdentityMicroservice.Repository;
using Microsoft.AspNetCore.Rewrite;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
var builder = WebApplication.CreateBuilder(args);
var Conf = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddJwt(Conf);
builder.Services.AddMongoDb(Conf);
builder.Services.AddTransient<IEncryptor, Encryptor>();
builder.Services.AddSingleton<IUserRepository>(sp =>
{
    return new UserRepository(sp.GetService<IMongoDatabase>() ?? throw new Exception("IMongoDatabase not found"));
});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "IdentityMicroservice", Version = "v1" });
});
builder.Services.AddSerilog(
    (_, config) =>
    {
        config
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Console();
    });

builder.Services.AddHealthChecks()
    .AddMongoDb(
        mongodbConnectionString: Conf["mongo:connectionString"]!,
        name: "mongo",
        failureStatus: HealthStatus.Unhealthy
    );
        
builder.Services.AddHealthChecksUI().AddInMemoryStorage();

var app = builder.Build();


app.UseSwagger();

app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "IdentityMicroservice v1"));

var option = new RewriteOptions();
option.AddRedirect("^$", "swagger");
app.UseRewriter(option);

app.UseHealthChecks("/healthz", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseHealthChecksUI();

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllers();
});
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
