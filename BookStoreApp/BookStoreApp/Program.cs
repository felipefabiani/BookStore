using BookStore.Helper;
using BookStoreApp.Client.Pages.Auth.Login;
using BookStoreApp.Components;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add MudBlazor services
builder.Services.AddMudServices();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddHttpClient(BookStoreConstants.Services.BookStoreApiName, (provider, client) =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    var apiUri = config["services:BookStore-Api:https:0"] ?? "https://localhost:7206";

    client.BaseAddress = new Uri($"{apiUri}/api/v1/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(BookStoreApp.Client._Imports).Assembly);

app.Run();
