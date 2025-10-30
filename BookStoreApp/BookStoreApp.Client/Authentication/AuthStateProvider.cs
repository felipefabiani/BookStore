using Blazored.LocalStorage;
using BookStore.Helper;
using BookStore.Models.Feature.Login;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace BookStoreApp.Client.Authentication;

public interface IAuthStateProvider
{
    int UserId { get; }

    Task<AuthenticationState> GetAuthenticationStateAsync();
    Task NotifyUserAuthentication(UserLoginResponse userLoggedin);
    Task NotifyUserLogout();
}

public class AuthStateProvider : AuthenticationStateProvider, IAuthStateProvider
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private readonly AuthenticationState _anonymous = new(
        new ClaimsPrincipal(new ClaimsIdentity()));

    private int _userId;
    public int UserId { get => _userId; }

    public AuthStateProvider(
        IHttpClientFactory httpClientFactory,
        ILocalStorageService localStorage)
    {
        _httpClient = httpClientFactory?
            .CreateClient(BookStoreConstants.Services.BookStoreApiName) 
            ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _localStorage = localStorage ?? throw new ArgumentNullException(nameof(localStorage));
    }
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var token = await _localStorage.GetItemAsync<string>(AuthConst.AuthToken);
            if (string.IsNullOrWhiteSpace(token))
            {
                return _anonymous!;
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthConst.Bearer, token);

            var authenticationState = GetAuthenticationState(token);

            await SetUserId(authenticationState);

            return authenticationState;
        }
        catch (Exception)
        {
            return _anonymous!;
        }
    }

    private async Task SetUserId(AuthenticationState authenticationState)
    {
        var userId = authenticationState.User.Claims.FirstOrDefault(x => x.Type == "id")?.Value ?? "0";
        await _localStorage.SetItemAsStringAsync("UserId", userId);
        _userId = int.Parse(userId);
    }

    public async Task NotifyUserAuthentication(UserLoginResponse userLoggedin)
    {
        try
        {
            await _localStorage.SetItemAsync(AuthConst.AuthToken, userLoggedin.Token.Value);
            var authenticatedUser = GetAuthenticationState(userLoggedin.Token.Value);
            var authState = Task.FromResult(authenticatedUser);

            await SetUserId(authenticatedUser);
            NotifyAuthenticationStateChanged(authState);
        }
        catch (Exception)
        {
            await NotifyUserLogout();
        }
    }

    private static AuthenticationState GetAuthenticationState(string token) => new(
        new ClaimsPrincipal(
            new ClaimsIdentity(
                JwtParser.ParseClaimsFromJWT(token),
                AuthConst.JwtAuthType)));
    public async Task NotifyUserLogout()
    {
        await _localStorage.RemoveItemAsync(AuthConst.AuthToken);

        var authState = Task.FromResult(_anonymous);
        NotifyAuthenticationStateChanged(authState!);
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }
}
