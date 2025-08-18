using BookStore.Api.Infrastructure;
using BookStore.Api.Model;
using BookStore.Database.Context;
using BookStore.Database.Infrastructure;
using BookStore.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.ServiceDiscovery;
using Serilog;

namespace BookStore.Api;

public partial class Program
{
    public static async Task Main()
    {
        try
        {
            
            var builder = WebApplication.CreateBuilder();

            builder.AddServiceDefaults();
            builder.Host.AddSerilog();
            // builder.AddSeqEndpoint("BookStore-Seq");

            Log.Information("Starting up the Minimal API application");

            builder.AddDatabase();

            // Add services to the container.
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            var app = builder.Build();

            app.MapDefaultEndpoints();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();

                Log.Information("Migration and Seed - {Environment}", SeedEnvironmentEnum.Dev);
                var factory = app.Services.GetRequiredService<IDbContextFactory<BookStoreContext>>();
                using var context = factory.CreateDbContext();
                await context.Seed(SeedEnvironmentEnum.Dev);
                Log.Information("Migration and Seed - done");
            }

            app.UseHttpsRedirection();

            var summaries = new[] {
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };

            app.MapGet("/weatherforecast", async (IDbContextFactory<BookStoreContext> contextFactory, ServiceEndpointResolver serviceEndpointResolver ) =>
            {
                Log.Information("Handling /weatherforecast request {TESTE}", "test");
                
                var forecast = Enumerable.Range(1, 5).Select(index =>
                {
                    var temp = Random.Shared.Next(-20, 55);
                    var summary = summaries[Random.Shared.Next(summaries.Length)];

                    Log.Debug("Generated forecast for day {Day}: {Temp}°C, {Summary}", index, temp, summary);

                    return new WeatherForecast(
                        DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        temp,
                        summary
                    );
                }).ToArray();

                Log.Information("Returning forecast with {Count} entries", forecast.Length);

                return forecast;
            })
            .WithName("GetWeatherForecast");

            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application startup failed: {Message} {@Ex}", ex.Message, ex);
        }
    }
}