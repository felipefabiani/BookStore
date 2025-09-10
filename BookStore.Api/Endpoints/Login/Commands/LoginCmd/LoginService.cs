
using Articles.Api.Infrastructure;
using BookStore.Database.Context;
using BookStore.Database.Entities;
using BookStore.Helper.Extensions;
using BookStore.Models.Auth;
using BookStore.Models.Feature.Login;
using Microsoft.EntityFrameworkCore;
using static BCrypt.Net.BCrypt;
using Ssc = System.Security.Claims;

namespace BookStore.Api.Endpoints.Login.Commands.LoginCmd;
public interface ILoginService
{
    Task<UserLoginResponse> Login(UserLoginRequest request, CancellationToken c);
}
public class LoginService : ILoginService, IScopedService, IAsyncDisposable, IDisposable
{
    private BookStoreReadOnlyContext _context;
    private readonly ILogger<LoginService> _logger;

    public LoginService(
        IDbContextFactory<BookStoreReadOnlyContext> context,
        ILogger<LoginService> logger)
    {
        _context = context?.CreateDbContext() ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public void Dispose()
    {
        _context?.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync().ConfigureAwait(false);
        _context = null!;
    }

    public async Task<UserLoginResponse> Login(UserLoginRequest request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Where(x => x.Email == request.Email)
            .Include(x => x.UserRoles)
            .ThenInclude(ur => ur.Role)
            .Include(x => x.Claims)
            .FirstOrDefaultAsync(cancellationToken) ?? new User();

        if (string.IsNullOrWhiteSpace(user.Password) ||
            !EnhancedVerify(request.Password, user.Password))
        {
            return NullUserLoginResponse.Empty;
        }

        return MapFromEntityAsync(user);

        UserLoginResponse MapFromEntityAsync(User e) => new()
        {
            FullName = $"{e.FirstName} {e.LastName}",
            UserRoles = [.. e.UserRoles.Select(x => x.Role.Name)],
            UserClaims = [.. e.Claims.Select(x => x.Name)],
            Token = new JwtToken
            {
                ExpiryDate = DateTime.UtcNow.AddHours(4),
                Value = DateTime.UtcNow.AddHours(4).CreateToken(
                    id: user.Id,
                    userName: $"{user.FirstName} {user.LastName}",
                    roles: [.. e.UserRoles.Select(x => x.Role.Name)],
                    claims: [.. e.Claims.Select(x => new Ssc.Claim(x.Name, x.Value))]
                )
            }
        };
    }
}
