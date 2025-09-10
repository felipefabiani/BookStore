using BookStore.Helper;
using BookStore.Models;
using BookStore.Models.Extentions;
using BookStore.Models.Feature.Login;
using BookStoreApp.Client.Shared.Templates.FormBases;
using System.Net.Http.Json;

namespace BookStoreApp.Client.Pages.Auth.Login;

public interface ILoginService : IService
{
    Task<Result<UserLoginResponse>> Login(UserLoginRequest userLogin);
}

public sealed class LoginService(IHttpClientFactory httpClientFactory) : Service, ILoginService
{
    // This class can be used to encapsulate any business logic related to the login process.
    // For example, you might want to add methods for validating user credentials,
    // logging login attempts, or interacting with a user database.
    private HttpClient _httpClient = httpClientFactory.CreateClient(BookStoreConstants.Services.BookStoreApiName);

    public async Task<Result<UserLoginResponse>> Login(UserLoginRequest userLogin)
    {
        var ret = await _httpClient.PostAsJsonAsync("login", userLogin, CancTokenSource.Token);

        var result = await ret.ToResult<UserLoginResponse>();
        return result;
    }
}

