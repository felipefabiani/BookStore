using BookStore.Helper;
using BookStore.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Api.Infrastructure;

public static class SetUpDatabaseExtension
{
    public static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        var connectionString = Environment.GetEnvironmentVariable(BookStoreConstants.ConnectionString);

        // AddDbContextBookStoreContext();
        // AddDbContextBookStoreReadOnlyContext();
        AddDbContextFactoryBookStoreContext();
        AddDbContextFactoryBookStoreReadOnlyContext();       
        return services;

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


