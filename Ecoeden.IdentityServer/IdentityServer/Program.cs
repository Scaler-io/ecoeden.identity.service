using IdentityServer;
using IdentityServer.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var logger = Logging.GetLogger(builder.Configuration, builder.Environment);

logger.Information("Current environment is {@env}", builder.Environment.EnvironmentName);

builder.Services.AddSingleton(x => logger);
builder.Host.UseSerilog(logger);

try
{   
    logger.Here().Information("Starting up");

    var app = builder
        .ConfigureServices()
        .ConfigurePipeline();

    if (app.Environment.IsDevelopment())
    {
        // this seeding is only for the template to bootstrap the DB and users.
        // in production you will likely want a different approach.
        logger.Here().Information("Seeding database...");
        SeedData.EnsureSeedData(app);
        logger.Here().Information("Done seeding database. Exiting.");
    }

    app.Run();
}
catch (Exception ex) when (ex is not HostAbortedException)
{
    logger.Here().Fatal(ex, "Unhandled exception");
}
finally
{
    logger.Here().Information("Shut down complete");
    Log.CloseAndFlush();
}