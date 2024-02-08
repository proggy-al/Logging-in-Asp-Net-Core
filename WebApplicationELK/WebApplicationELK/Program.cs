using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Formatting.Json;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.File;
using Elastic.Apm.NetCoreAll;
using SeriLog.LogSanitizingFormatter;
using WebApplicationELK.Infrastructure;
using WebApplicationELK.Services;
using Destructurama;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHostedService<LifetimeEventsHostedService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//serilog
var customRules = new List<ISanitizingFormatRule> { new DemoSanitzingRule() }; //example of sanitizing
var logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration)
                        .Enrich.FromLogContext()
                        .WriteTo.Console(new JsonLogSanitizingFormatter(customRules,new JsonFormatter(), true))  //example of sanitizing

                        //.WriteTo.EventLog("Otus", manageEventSource: true)

                        .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://elasticsearch-demo:9200"))
                        {

                            FailureCallback = e =>
                            {
                                Console.WriteLine("Unable to submit event " + e.Exception);
                            },
                            FailureSink = new FileSink("./failures.txt", new JsonFormatter(), null),
                            //ModifyConnectionSettings = x => x.BasicAuthentication("elastic", "KXjnP*zPjJyp0hUw4yoQ"),

                            TypeName = null,

                            IndexFormat = "otus-log",
                            AutoRegisterTemplate = true,
                            EmitEventFailure = EmitEventFailureHandling.ThrowException | EmitEventFailureHandling.RaiseCallback | EmitEventFailureHandling.WriteToSelfLog
                        })
                        .MinimumLevel.Information()
                        .Destructure.UsingAttributes()
                        .CreateLogger(); 

builder.Services.AddSerilog(logger);

//logging level
//builder.Logging.SetMinimumLevel(LogLevel.Trace);


var app = builder.Build();


// retrieve the logger
var logger1 = app.Services.GetService<ILogger<Program>>();

logger1.LogDebug("Some debug information");

logger1.LogInformation("Changing data in log to console dog - cat");


app.UseAllElasticApm(builder.Configuration);


app.UseSwagger();
app.UseSwaggerUI();


app.UseAuthorization();

app.MapControllers();

app.Run();
