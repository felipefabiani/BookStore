using Microsoft.Extensions.DependencyInjection;

namespace BookStore.Helper.Auth
{
    public static class PolicyExtension
    {
        public static IServiceCollection AddBookStoreAuthorization(this IServiceCollection services) => services.AddAuthorizationCore(op => {
            op.AddPolicy(Policies.Author.AuthorSaveArticle, Policies.Author.GetAuthorSaveArticle());
            op.AddPolicy(Policies.Author.Read, Policies.Author.GetReadAuthor());
        });
    }
}
