using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebsiteManagement.Application.Common;
using WebsiteManagement.Application.Interfaces;
using WebsiteManagement.Common;
using WebsiteManagement.Infrastructure.Persistence.EntityConfigurations;

namespace WebsiteManagement.Infrastructure.Persistence
{
    public class WebsiteManagementDbContext : DbContext, IUnitOfWork
    {
        public WebsiteManagementDbContext(DbContextOptions<WebsiteManagementDbContext> options)
            : base(options)
        {
        }

        public DbSet<Domain.Website> Websites { get; set; }

        public async Task CommitAsync(CancellationToken cancellationToken)
        {
            try
            {
                await SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null &&
                    ex.InnerException.Message.Contains("Cannot insert duplicate key row in object 'dbo.websites' with unique index 'IX_websites_Url'"))
                {
                    throw new UrlExistsException(ex.Message, ex);
                }

                throw;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new WebsiteEntityConfigurations());
            modelBuilder.ApplyConfiguration(new CategoryEntityConfigurations());
            modelBuilder.ApplyConfiguration(new ImageEntityConfigurations());
        }
    }
}
