using Common;
using NLog;
using NLog.Web;
using EmpdataConsumer;
using Microsoft.EntityFrameworkCore;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
try
{

    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddSignalR();
    builder.Services.AddSingleton<EmployeeHub>();
    builder.Services.AddDbContext<EmployeeDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("EmployeeDB")));


    builder.Services.AddDbContext<EmployeeDbContext>();
    builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
    builder.Services.AddHostedService<MessageConsumer>();

    builder.Host.UseNLog();

    var app = builder.Build();

    app.MapHub<EmployeeHub>("/notifyEmployee");
    app.MapGet("/", () => "Publish Employees Data to SignalR");

    app.Run();
}
catch (Exception exception)
{
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    LogManager.Shutdown();
}