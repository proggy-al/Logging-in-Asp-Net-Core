using Serilog.Formatting.Json;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.File;
using Serilog;
using Elastic.Apm.NetCoreAll;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration)
                        .Enrich.FromLogContext()
                        .WriteTo.Console()  
                        .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://elasticsearch-demo:9200"))
                        {

                            FailureCallback = e =>
                            {
                                Console.WriteLine("Unable to submit event " + e.Exception);
                            },
                            FailureSink = new FileSink("./failures2.txt", new JsonFormatter(), null),

                            TypeName = null,

                            IndexFormat = "otus-log-service2",
                            AutoRegisterTemplate = true,
                            EmitEventFailure = EmitEventFailureHandling.ThrowException | EmitEventFailureHandling.RaiseCallback | EmitEventFailureHandling.WriteToSelfLog
                        })
                        .MinimumLevel.Information()
                        .CreateLogger();

builder.Services.AddSerilog(logger);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.UseAllElasticApm(builder.Configuration);

app.Run();
