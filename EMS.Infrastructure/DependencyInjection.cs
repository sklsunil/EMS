using EMS.Application.Common.Interfaces;
using EMS.Infrastructure.Extensions;
using EMS.Infrastructure.Persistence.Interceptors;

using Microsoft.Extensions.Configuration;

namespace EMS.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<AuditableEntitySaveChangesInterceptor>();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                var isInMemoryDatabase = Convert.ToBoolean(configuration["UseInMemoryDatabase"]);
                if (isInMemoryDatabase)
                {
                    options.UseInMemoryDatabase("EMS");
                }
                else
                {
                    options.UseSqlServer(
                          configuration.GetConnectionString("DefaultConnection"),
                          builder =>
                          {
                              builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                              builder.EnableRetryOnFailure(maxRetryCount: 5,
                                                           maxRetryDelay: TimeSpan.FromSeconds(10),
                                                           errorNumbersToAdd: null);
                              builder.CommandTimeout(15);
                          });
                }
                options.EnableDetailedErrors(detailedErrorsEnabled: true);
                options.EnableSensitiveDataLogging();
            });

            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddScoped<IDbContextFactory<ApplicationDbContext>, EMSContextFactory<ApplicationDbContext>>();
            services.AddTransient<IApplicationDbContext>(provider => provider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>().CreateDbContext());
            services.AddScoped<ApplicationDbContextInitialiser>();

            services.AddServices();
            services.AddControllers();
            return services;
        }
    }
}
