﻿using Microsoft.AspNetCore.Authorization;

namespace BookStore.Helper.Auth;

public static class Policies
{
    public static class Author
    {
        public static readonly string AuthorSaveArticle = "AuthorSaveArticle";
        public static AuthorizationPolicy GetAuthorSaveArticle()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireRole("Author")
                .RequireClaim("Author_Save_Own")
                .Build();
        }

        public static readonly string Read = "ReadAuthor";
        public static AuthorizationPolicy GetReadAuthor()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireRole("Author")
                .RequireClaim("Author_Get_Own_List")
                .Build();
        }
    }
}
