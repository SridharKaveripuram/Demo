using EmpDataProducer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<MessageProducer>();
var app = builder.Build();


app.MapGet("/", () => "Publish Employee Data");

app.Run();
