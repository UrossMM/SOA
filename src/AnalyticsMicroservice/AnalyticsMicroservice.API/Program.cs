using AnalyticsMicroservice.API.Rabbit;
using AnalyticsMicroservice.API.Repository;
using MongoDB.Driver;

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

var connectionString = "mongodb://analyticsmongo:27017";
var client = new MongoClient(connectionString);
builder.Services.AddSingleton<IMongoClient>(client);
builder.Services.AddScoped<IAnalyticsRepository, AnalyticsRepository>();

/*builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
});*/

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

Receiver r = new Receiver();
r.Subscribe("sensor/data");

app.Run();
