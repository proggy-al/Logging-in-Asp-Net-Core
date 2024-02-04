using WebApplicationShowLogging.Infrasturcture;
using WebApplicationShowLogging.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<LifetimeEventsHostedService>();
// Register custom logger
builder.Services.AddSingleton(typeof(ILogger<>), typeof(MyCustomLogger<>));
//builder.Services.AddLogging();


//logging level
builder.Logging.SetMinimumLevel(LogLevel.Trace);

var app = builder.Build();


// retrieve the logger
var logger = app.Services.GetService<ILogger<Program>>();

logger.LogDebug("Some debug information");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
