//using BookStore.Models.Auth;
//using BookStore.Models.Feature.Login;
//using BookStoreApp.Client.Shared.Templates.FormBases;
//using LanguageExt;

//namespace BookStore.Api.Endpoints.Login.Commands.LoginCmd;

//public interface ILoginService : IService
//{
//    Task<Fin<UserLoginResponse>> Login(UserLoginRequest userLogin);
//}

//public class LoginService : ILoginService
//{
//    // This class can be used to encapsulate any business logic related to the login process.
//    // For example, you might want to add methods for validating user credentials,
//    // logging login attempts, or interacting with a user database.

//    public async Task<Fin<UserLoginResponse>> Login(UserLoginRequest userLogin)
//    {
//        return await Task.FromResult(new UserLoginResponse
//        {
//            FullName = "Felipe Fabiani",
//            UserClaims = ["Claim1", "Claim2"],
//            UserRoles = ["Admin", "User"],
//            Token = new JwtToken
//            {
//                Value = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkZlbGlwZSBBZmFiaWFuaSIsImlhdCI6MTUxNjIzOTAyMn0.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c",
//                ExpiryDate = DateTime.UtcNow.AddHours(1)
//            }
//        });
//    }
//}

