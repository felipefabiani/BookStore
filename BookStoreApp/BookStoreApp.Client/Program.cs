using BookStore.Helper;
using BookStore.Models.Auth;
using BookStoreApp.Client.Infrastructure;
using BookStoreApp.Client.Pages.Auth.Login;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddMudServices();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddFluentValidators([new JwtToken().GetType().Assembly.GetName().Name]);

builder.Services.AddHttpClient(BookStoreConstants.Services.BookStoreApiName, (provider, client) =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    var apiUri = config["services:BookStore-Api:https:0"] ?? "https://localhost:7206";

    client.BaseAddress = new Uri($"{apiUri}/api/v1/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});


await builder.Build().RunAsync();
