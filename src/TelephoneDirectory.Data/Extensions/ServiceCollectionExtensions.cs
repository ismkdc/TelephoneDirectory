using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TelephoneDirectory.Data.Entities;

namespace TelephoneDirectory.Data.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDbContext(this IServiceCollection services,
        string? connectionString)
    {
        ArgumentException.ThrowIfNullOrEmpty(connectionString);

        services.AddDbContextPool<TelephoneDirectoryContext>(
            options =>
                options.UseNpgsql(connectionString)
        );

        return services;
    }
}