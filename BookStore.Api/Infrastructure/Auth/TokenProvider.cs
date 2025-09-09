using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Entities = BookStore.Database.Entities;
using JsonWebTokens = Microsoft.IdentityModel.JsonWebTokens;

namespace BookStore.Api.Infrastructure.Auth;

public interface ITokenProvider
{
    string Create(Entities.User user);
}

public sealed class TokenProvider(IConfiguration configuration, IOptions<JwtSettings> jwtOptions) : ITokenProvider
{
    public string Create(Entities.User user)
    {
        var secret = configuration["jwt-secret"]!;
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var jwtSettings = jwtOptions.Value;

        var tokenDescription = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(JwtRegisteredClaimNames.Sub, user.FirstName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
            ]),
            Expires = DateTime.UtcNow.AddMinutes(jwtSettings.Expirationminutes),
            SigningCredentials = credentials,
            Issuer = jwtSettings.Issuer,
            Audience = jwtSettings.Audience
        };

        var tokenHandler = new JsonWebTokens.JsonWebTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescription);

        return token;
    }
}
