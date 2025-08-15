using BookStore.Api.Infrastructure;
using BookStore.Api.Model;
using BookStore.Database.Context;
using BookStore.Database.Infrastructure;
using Microsoft.EntityFrameworkCore;
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

            Log.Information("Starting up the Minimal API application");

            builder.Services.AddDatabase();

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

            var summaries = new[]
            {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

            app.MapGet("/weatherforecast", (IDbContextFactory<BookStoreContext> contextFactory) =>
            {
                var forecast = Enumerable.Range(1, 5).Select(index =>
                    new WeatherForecast
                    (
                        DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        Random.Shared.Next(-20, 55),
                        summaries[Random.Shared.Next(summaries.Length)]
                    ))
                    .ToArray();
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