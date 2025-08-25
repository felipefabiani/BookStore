using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using BookStore.Api.Infrastructure;
using BookStore.Api.Infrastructure.Auth;
using BookStore.Api.Model;
using BookStore.Database.Context;
using BookStore.Database.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.ServiceDiscovery;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Serilog;
using System.Text;

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
            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
            builder.Services.AddSingleton<ITokenProvider, TokenProvider>();
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

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


            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false; // Set to true in production
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["JwtSettings:Audience"],
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero, // Remove delay of token expiration
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwt-secret"]!)),
                    };
                });
            builder.Services.AddAuthorization();

            builder.Services.AddOpenApi();
            var app = builder.Build();

            app.MapDefaultEndpoints();

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

                    options
                        .AddPreferredSecuritySchemes("OAuth2")
                        .AddPasswordFlow("OAuth2", flow =>
                        {

                            flow.Username = "test";
                            flow.Password = "123";
                            flow.SelectedScopes = ["profile", "email"];
                        });
                });

                //Log.Information("Migration and Seed - {Environment}", SeedEnvironmentEnum.Dev);
                //var factory = app.Services.GetRequiredService<IDbContextFactory<BookStoreContext>>();
                //using var context = factory.CreateDbContext();
                //await context.Seed(SeedEnvironmentEnum.Dev);
                //Log.Information("Migration and Seed - done");
            }

            app.UseHttpsRedirection();
            app.UseRouting();            // Optional in Minimal API, but safe to include
            app.UseAuthentication();     // Must come before authorization
            app.UseAuthorization();      // Required for [Authorize] or RequireAuthorization()

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

            weatherGroup.MapGet("/Test", (
                IDbContextFactory<BookStoreContext> contextFactory,
                ServiceEndpointResolver serviceEndpointResolver) =>
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


            var authGroup = routGroup.MapGroup("auth");

            authGroup.MapPost("/login", (
                UserLoginDto login,
                ITokenProvider tp) =>
            {
                // Replace with real user validation
                if (login.Username != "test" || login.Password != "123")
                    return Results.Unauthorized();

                var testUser = new User
                {

                    Email = login.Username,
                    Password = login.Password,
                };

                var token = tp.Create(testUser);
                return Results.Ok(token);
            })
                .WithName("Login")
                .MapToApiVersion(2)
                .MapToApiVersion(3);

            authGroup.MapGet("/secure-data", [Authorize] () =>
            {
                return Results.Ok("This is protected data");
            }).WithName("Data1");
            authGroup.MapGet("/secure-data2", () =>
            {
                return Results.Ok("This is protected data");
            }).WithName("Data2")
            .RequireAuthorization();



            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application startup failed: {Message} {@Ex}", ex.Message, ex);
        }
    }
}

public record UserLoginDto(string Username, string Password);
