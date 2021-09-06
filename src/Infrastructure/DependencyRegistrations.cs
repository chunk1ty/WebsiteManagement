using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebsiteManagement.Application.Interfaces;
using WebsiteManagement.Infrastructure.Identity;
using WebsiteManagement.Infrastructure.Persistence;
using WebsiteManagement.Infrastructure.Security;

namespace WebsiteManagement.Infrastructure
{
    public static class DependencyRegistrations
    {
        public static void AddInfrastructure(this IServiceCollection services, string connectionString)
        {
            // services.AddDbContext<WebsiteManagementDbContext>(options => options.UseSqlServer(connectionString), ServiceLifetime.Scoped);
            services.AddDbContext<WebsiteManagementDbContext>(options => options.UseInMemoryDatabase("WebsiteManagementDbContext"), ServiceLifetime.Scoped);
            services.AddScoped<IUnitOfWork>(provider => provider.GetService<WebsiteManagementDbContext>());
            services.AddScoped<IWebsiteRepository, WebsiteRepository>();

            services.AddSingleton<ICypher, RijndaelPbkdf2Cypher>();

            services.AddSingleton<IAuthenticationService, AuthenticationService>();
        }
    }
}
