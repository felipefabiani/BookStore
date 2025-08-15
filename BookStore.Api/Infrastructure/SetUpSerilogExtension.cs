using BookStore.Database.Context;
using BookStore.Helper;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BookStore.Api.Infrastructure;

public static class SetUpSerilogExtension
{
    public static IHostBuilder AddSerilog(this IHostBuilder host)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
            .Build();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        host.UseSerilog();

        return host;
    }
}


