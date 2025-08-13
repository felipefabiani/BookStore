namespace BookStore.Database.Context;

public class BookStoreOptions
{
    public string SaltId { get; set; } = string.Empty;
    public string JwtSigningKey { get; set; } = string.Empty;
}
