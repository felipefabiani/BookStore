using BookStore.Models.Feature.Login;
using Microsoft.AspNetCore.Components.Authorization;

namespace BookStoreApp.Client.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IAuthStateProvider _authStateProvider;

    public AuthenticationService(AuthenticationStateProvider authStateProvider)
    {
        _authStateProvider = (IAuthStateProvider)authStateProvider ?? throw new ArgumentNullException(nameof(authStateProvider));
    }

    public async Task<UserLoginResponse> Login(UserLoginResponse userForAuthentication)
    {
        await _authStateProvider.NotifyUserAuthentication(userForAuthentication);
        return userForAuthentication;
    }

    public async Task Logout()
    {
        await _authStateProvider.NotifyUserLogout();
    }
}
