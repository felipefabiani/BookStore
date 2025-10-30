using Blazored.LocalStorage;
using BookStore.Helper;
using BookStore.Models.Auth;
using BookStoreApp.Client.Authentication;
using BookStoreApp.Client.Infrastructure;
using BookStoreApp.Client.Pages.Auth.Login;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.BrowserConsole()
    .CreateLogger();

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// builder.Logging.AddSerilog();
builder.Services.AddMudServices();

builder.Services.AddFluentValidators([new JwtToken().GetType().Assembly.GetName().Name]);
builder.Services.AddScoped<ILoginService, LoginService>();
// builder.Services.AddBookStoreAuthorization();
 builder.Services.AddScoped<IAuthStateProvider, AuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(p => (AuthStateProvider)p.GetRequiredService<IAuthStateProvider>());
builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
builder.Services.AddBlazoredLocalStorage();


builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddTransient<LoggingHandler>();
builder.Services.AddHttpClient(BookStoreConstants.Services.BookStoreApiName, (provider, client) =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    var apiUri =
        config["services:BookStore-Api:https:0"] ??
        "https://localhost:7206";

    client.BaseAddress = new Uri($"{apiUri}/api/v1/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
})
    .AddHttpMessageHandler<LoggingHandler>(); ;


await builder.Build().RunAsync();
