using BookStore.Helper.Auth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using static BCrypt.Net.BCrypt;
using Ssc = System.Security.Claims;

namespace BookStore.Helper.Extensions;

public static class StringExtension
{
    public static string GetPassword(this string pwd) => EnhancedHashPassword(pwd, 12);

    public static string CreateToken(
        this DateTime expireAt,
        int id,
        string userName,
        IEnumerable<string>? roles = null,
        IEnumerable<Ssc.Claim>? claims = null,
        IEnumerable<string>? permissions = null)
    {
        var list = new List<Ssc.Claim>
        {
            new Ssc.Claim(Ssc.ClaimTypes.Name, userName),
            new Ssc.Claim("id", $"{id}")
        };

        if (claims != null)
        {
            list.AddRange(claims);
        }

        if (permissions != null)
        {
            list.AddRange(permissions.Select((string p) => new Ssc.Claim("permissions", p)));
        }

        if (roles != null)
        {
            list.AddRange(roles.Select((string r) => new Ssc.Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", r)));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new Ssc.ClaimsIdentity(list),
            Expires = expireAt,
            // SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(signingKey.ToString())), "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256")
            SigningCredentials = new SigningIssuerCertificate().GetAudienceSigningKey(),
            Issuer = BookStoreConstants.Security.Issuer,
            Audience = BookStoreConstants.Security.Audience
        };
        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        jwtSecurityTokenHandler.OutboundClaimTypeMap.Clear();
        return jwtSecurityTokenHandler.WriteToken(jwtSecurityTokenHandler.CreateToken(tokenDescriptor));
    }
}