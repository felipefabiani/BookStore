using BookStore.Helper;
using BookStore.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Api.Infrastructure;

public static class SetUpDatabaseExtension
{
    public static WebApplicationBuilder AddDatabase(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        
        var connectionString = builder.Configuration
            .GetConnectionString(BookStoreConstants.Services.BookStoreDatabaseName);

        AddDbContextFactoryBookStoreContext();
        AddDbContextFactoryBookStoreReadOnlyContext();       
        return builder;

        void AddDbContextFactoryBookStoreContext()
        {
            services.AddDbContextFactory<BookStoreContext>(options =>
            {
                options
#if DEBUG
                    .EnableSensitiveDataLogging()
#endif
                    .UseSqlServer(connectionString);
            });
        }
        void AddDbContextFactoryBookStoreReadOnlyContext()
        {
            services.AddDbContextFactory<BookStoreReadOnlyContext>(options =>
            {
                options
#if DEBUG
                    .EnableSensitiveDataLogging()
#endif
                    .UseSqlServer(connectionString);
            });
        }
    }
}


