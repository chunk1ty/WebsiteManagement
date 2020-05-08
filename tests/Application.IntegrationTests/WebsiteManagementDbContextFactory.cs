using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using WebsiteManagement.Infrastructure.Persistence;

namespace WebsiteManagement.Application.IntegrationTests
{
    // used to initialize WebsiteManagementIntegrationTestsDbContext 
    public class WebsiteManagementDbContextFactory : IDesignTimeDbContextFactory<WebsiteManagementDbContext>
    {
        public WebsiteManagementDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<WebsiteManagementDbContext>();
            optionsBuilder.UseSqlServer(Configuration.Config.GetValue<string>("ConnectionStrings:DatabaseConnection"));
            return new WebsiteManagementDbContext(optionsBuilder.Options);
        }
    }
}
