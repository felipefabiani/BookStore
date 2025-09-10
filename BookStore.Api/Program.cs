using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using BookStore.Api.Endpoints.Login.Commands.LoginCmd;
using BookStore.Api.Infrastructure;
using BookStore.Api.Infrastructure.Auth;
using BookStore.Api.Infrastructure.Middlewares;
using BookStore.Database.Context;
using BookStore.Database.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Serilog;
using System.Reflection;
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
            builder.Services.AddScoped<ILoginService, LoginService>();
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

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy
                    // WithOrigins("https://localhost:58725;http://localhost:5088")
                          .AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

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

                Log.Information("Migration and Seed - {Environment}", SeedEnvironmentEnum.Dev);
                var factory = app.Services.GetRequiredService<IDbContextFactory<BookStoreContext>>();
                using var context = factory.CreateDbContext();
                await context.Seed(SeedEnvironmentEnum.Dev);
                Log.Information("Migration and Seed - done");
            }

            app.UseCors("AllowFrontend");
            app.UseHttpsRedirection();
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseRouting();            // Optional in Minimal API, but safe to include
            app.UseAuthentication();     // Must come before authorization
            app.UseAuthorization();      // Required for [Authorize] or RequireAuthorization()
            app.RegisterEndpoints();

            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application startup failed: {Message} {@Ex}", ex.Message, ex);
        }
    }
}

public record UserLoginDto(string Username, string Password);
