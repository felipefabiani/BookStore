namespace BookStore.Api.Infrastructure.Auth;

public class JwtSettings
{
    public int Expirationminutes { get; set; } = 5;
    public string Issuer { get; set; } = "BookStore.Api";
    public string Audience { get; set; } = "BookStore.Api";
}
