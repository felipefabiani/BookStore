using BookStore.Helper;
using BookStore.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Database.Infrastructure;

public static class SetUpDatabaseExtension
{
    public static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        var connectionString = Environment.GetEnvironmentVariable(BookStoreConstants.ConnectionString);

        AddDbContextFactoryBookStoreContext();
        AddDbContextFactoryBookStoreReadOnlyContext();
        AddDbContextBookStoreReadOnlyContext();
        AddDbContextBookStoreContext();
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
        void AddDbContextBookStoreReadOnlyContext()
        {
            services.AddDbContext<BookStoreReadOnlyContext>(options =>
            {
                options
#if DEBUG
                    .EnableSensitiveDataLogging()
#endif
                    .UseSqlServer(connectionString, x =>
                    {
                        x.MigrationsAssembly(typeof(BookStoreContext).Assembly.FullName);
                        x.EnableRetryOnFailure(2);
                    });
            });
        }
        void AddDbContextBookStoreContext()
        { 
            services.AddDbContext<BookStoreContext>(options =>
            {
                options
#if DEBUG
                    .EnableSensitiveDataLogging()
#endif
                    .UseSqlServer(connectionString, x =>
                    {
                        x.MigrationsAssembly(typeof(BookStoreContext).Assembly.FullName);
                        x.EnableRetryOnFailure(2);
                    });

            });
        }
    }
}


