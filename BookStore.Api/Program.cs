using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using BookStore.Api.Infrastructure;
using BookStore.Api.Model;
using BookStore.Database.Context;
using BookStore.Database.Infrastructure;
using BookStore.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.ServiceDiscovery;
using Scalar.AspNetCore;
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


            builder.Services.AddOpenApi();
            builder.Services.AddOpenApi("v2");
            builder.Services.AddOpenApi("v3");
            builder.Services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(2, 0);

                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
                options.UnsupportedApiVersionStatusCode = 404;
                //ApiVersionReader.Combine(
                //    new HeaderApiVersionReader("X-ApiVersion"),
                //    new UrlSegmentApiVersionReader());

            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            })
            .EnableApiVersionBinding();
            builder.Services.AddEndpointsApiExplorer();

            // builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());

            // Add services to the container.
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

            // builder.Services.AddAuthentication().AddJwtBearer();

            builder.Services.AddOpenApi();
            var app = builder.Build();

            app.MapDefaultEndpoints();

            var apiVersionSet = app.NewApiVersionSet()
               .HasDeprecatedApiVersion(new ApiVersion(1))
               .HasApiVersion(new ApiVersion(2))
               .HasApiVersion(new ApiVersion(3))
               .ReportApiVersions()
               .Build();

            var routGroup = app.MapGroup("api/v{apiVersion:apiVersion}")
                .WithApiVersionSet(apiVersionSet)
                .HasDeprecatedApiVersion(1)
                .HasApiVersion(2)
                .HasApiVersion(3);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference(options =>
                {
                    var apiVersion = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

                    foreach (var description in apiVersion.ApiVersionDescriptions)
                {
                        options.AddDocument(
                            description.GroupName.ToString(),
                            $"BookStore API {description.GroupName}",
                            $"openapi/{description.GroupName}.json");
                    }
                });

                //Log.Information("Migration and Seed - {Environment}", SeedEnvironmentEnum.Dev);
                //var factory = app.Services.GetRequiredService<IDbContextFactory<BookStoreContext>>();
                //using var context = factory.CreateDbContext();
                //await context.Seed(SeedEnvironmentEnum.Dev);
                //Log.Information("Migration and Seed - done");
            }

            app.UseHttpsRedirection();

            var summaries = new[] {
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };


            var apiVersionSet = app.NewApiVersionSet()
               .HasDeprecatedApiVersion(new ApiVersion(1))
               .HasApiVersion(new ApiVersion(2))
               .HasApiVersion(new ApiVersion(3))
               .ReportApiVersions()
               .Build();

            var routGroup = app.MapGroup("api/v{apiVersion:apiVersion}")
                .WithApiVersionSet(apiVersionSet)
                .HasDeprecatedApiVersion(1)
                .HasApiVersion(2)
                .HasApiVersion(3);
            var weatherGroup = routGroup.MapGroup("weather");

            weatherGroup.MapGet("/weatherforecast", async (IDbContextFactory<BookStoreContext> contextFactory, ServiceEndpointResolver serviceEndpointResolver) =>
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
                        summary,
                        "Api V1"
                    );
                }).ToArray();

                Log.Information("Returning forecast with {Count} entries", forecast.Length);

                return forecast;
            })
            .WithName("GetWeatherForecastV1")
            .MapToApiVersion(1);

            weatherGroup.MapGet("/user/get", async (IDbContextFactory<BookStoreContext> contextFactory, ServiceEndpointResolver serviceEndpointResolver) =>
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
                        summary,
                        "Api V1"
                    );
                }).ToArray();

                Log.Information("Returning forecast with {Count} entries", forecast.Length);

                return forecast;
            })
            .WithName("UserGetV1")
            .MapToApiVersion(1);

            weatherGroup.MapGet("/weatherforecast", async (IDbContextFactory<BookStoreContext> contextFactory, ServiceEndpointResolver serviceEndpointResolver) =>
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
                        summary,
                        "Api V2"
                    );
                }).ToArray();

                Log.Information("Returning forecast with {Count} entries", forecast.Length);

                return forecast;
            })
            .WithName("GetWeatherForecastV2")
            .MapToApiVersion(2)
            .MapToApiVersion(3);

            weatherGroup.MapGet("/Test", async (IDbContextFactory<BookStoreContext> contextFactory, ServiceEndpointResolver serviceEndpointResolver) =>
            {
                Log.Information("Handling /weatherforecast request {TESTE}", "test");
                
                var forecast = Enumerable.Range(1, 5).Select(index =>
                {
                    var temp = Random.Shared.Next(-20, 55);
                    var summary = summaries[Random.Shared.Next(summaries.Length)];

                    Log.Debug("Generated forecast for day {Day}: {Temp}�C, {Summary}", index, temp, summary);

                    return new WeatherForecast(
                        DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        temp,
                        summary,
                        "Api V2"
                    );
                }).ToArray();

                Log.Information("Returning forecast with {Count} entries", forecast.Length);

                return forecast;
            })
            .WithName("Test")
            .MapToApiVersion(2)
            .MapToApiVersion(3);

            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application startup failed: {Message} {@Ex}", ex.Message, ex);
        }
    }
}