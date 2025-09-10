using BookStore.Models.Feature.Login;

namespace BookStoreApp.Client.Authentication;

public interface IAuthenticationService
{
    Task<UserLoginResponse> Login(UserLoginResponse userForAuthentication);
    Task Logout();
}
