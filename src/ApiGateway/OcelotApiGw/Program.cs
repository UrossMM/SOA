using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOcelot();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
    });
});

var app = builder.Build();


builder.WebHost.ConfigureAppConfiguration(config => config.AddJsonFile("ocelot.Development.json"));

app.MapGet("/", () => "Hello World!");

app.UseCors();

await app.UseOcelot();

app.Run();

