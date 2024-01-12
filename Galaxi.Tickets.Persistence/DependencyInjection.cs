using Galaxi.Tickets.Persistence.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Galaxi.Tickets.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TicketContextDb>(options =>
                options.UseSqlServer(
                    configuration["connectionStrings:TickectsEntities"] ?? string.Empty,
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(typeof(TicketContextDb).Assembly.FullName);
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 10,
                            maxRetryDelay: TimeSpan.FromSeconds(10),
                            errorNumbersToAdd: null
                        );
                    }));
            services.AddScoped<ITicketContextDb>(provider => provider.GetRequiredService<TicketContextDb>());
            return services;
        }
    }
}
