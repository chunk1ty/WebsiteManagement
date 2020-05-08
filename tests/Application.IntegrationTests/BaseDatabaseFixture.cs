using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using WebsiteManagement.Infrastructure;
using WebsiteManagement.Infrastructure.Persistence;

namespace WebsiteManagement.Application.IntegrationTests
{
    [TestFixture]
    public abstract class BaseDatabaseFixture
    {
        private ServiceProvider _container;

        [OneTimeSetUp]
        public void  OneTimeSetUp()
        {
            var services = new ServiceCollection();

            services.AddApplication();
            services.AddInfrastructure(Configuration.Config.GetValue<string>("ConnectionStrings:DatabaseConnection"));

            _container = services.BuildServiceProvider(validateScopes: true);
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            using (WebsiteManagementDbContext db = CreateScope().ServiceProvider.GetService<WebsiteManagementDbContext>())
            {
                db.Database.ExecuteSqlRaw("delete from [dbo].[websites]");
            }
        }

        protected IServiceScope CreateScope()
        {
            return _container.CreateScope();
        }
    }
}