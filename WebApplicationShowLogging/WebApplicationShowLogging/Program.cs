using Serilog;
using Serilog.Formatting.Json;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.File;
using WebApplicationShowLogging.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<LifetimeEventsHostedService>();

// Register custom logger
//builder.Services.AddSingleton(typeof(ILogger<>), typeof(MyCustomLogger<>));

//setup ILoggerFactory
builder.Services.AddLogging(builder => builder.AddConsole());
builder.Logging.AddConsole();
//builder.Logging.AddEventLog(config=> config.SourceName="CustomLog");



//logging level
builder.Logging.SetMinimumLevel(LogLevel.Trace);


//serilog
builder.Host.UseSerilog((context, services, configuration) =>
                    configuration
                        .ReadFrom.Configuration(context.Configuration)
                        .Enrich.FromLogContext()
                        .WriteTo.Console()

                        //.WriteTo.EventLog("Otus", manageEventSource: true)

                        .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://elasticsearchDemo:9200"))
                        {

                            FailureCallback = e =>
                            {
                                Console.WriteLine("Unable to submit event " + e.Exception);
                            },
                            FailureSink = new FileSink("./failures.txt", new JsonFormatter(), null),
                            ModifyConnectionSettings = x => x.BasicAuthentication("elastic", "KXjnP*zPjJyp0hUw4yoQ"),

                            TypeName = null,

                            IndexFormat = "otus-log",
                            AutoRegisterTemplate = true,
                            EmitEventFailure = EmitEventFailureHandling.ThrowException | EmitEventFailureHandling.RaiseCallback | EmitEventFailureHandling.WriteToSelfLog
                        })) ;

var app = builder.Build();


// retrieve the logger
var logger = app.Services.GetService<ILogger<Program>>();

logger.LogDebug("Some debug information");


// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
