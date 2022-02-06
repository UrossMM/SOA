using CommandMicroservice.API.Commander;
using CommandMicroservice.API.Hubs;
using CommandMicroservice.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
    });
});

builder.Services.AddSignalR(options => options.EnableDetailedErrors = true);

Hivemq mqtt = new Hivemq();
CommandHub hub = new CommandHub();
builder.Services.AddSingleton(mqtt);
builder.Services.AddSingleton(new DataCommander(mqtt, hub));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.MapHub<CommandHub>("/commandhub");

app.Run();
