using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOcelot();

var app = builder.Build();


builder.WebHost.ConfigureAppConfiguration(config => config.AddJsonFile("ocelot.Development.json"));

app.MapGet("/", () => "Hello World!");

await app.UseOcelot();

app.Run();

