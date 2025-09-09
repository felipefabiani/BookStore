using BookStoreApp.Client.Pages.Auth.Login;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddMudServices();
builder.Services.AddScoped<ILoginService, LoginService>();

//builder.Services.AddHttpClient(BookStoreConstants.Services.BookStoreApiName, (provider, client) =>
//{                                                 // services__BookStore-Api__https__0
//    //var apiUri = Environment.GetEnvironmentVariable("services__BookStore-Api__https__0")!;
//    //var test = new Uri($"https+http://{"BookStore-Api__https__0"}/api/v1/");
//    //client.BaseAddress = new Uri($"{apiUri}/api/v1/");
//    //client.DefaultRequestHeaders.Add("Accept", "application/json");

//    var config = provider.GetRequiredService<IConfiguration>();
//    var apiUri = config["services:BookStore-Api:https:0"];

//    foreach (var kvp in Environment.GetEnvironmentVariables().Cast<DictionaryEntry>())
//    {
//        Console.WriteLine($"{kvp.Key} = {kvp.Value}");
//    }

//    foreach (var kvp in config.AsEnumerable())
//    {
//        Console.WriteLine($"{kvp.Key} = {kvp.Value}");
//    }

//    client.BaseAddress = new Uri($"{apiUri}/api/v1/");
//    client.DefaultRequestHeaders.Add("Accept", "application/json");
//});


await builder.Build().RunAsync();
