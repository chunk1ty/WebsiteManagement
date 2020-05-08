using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebsiteManagement.Domain;

namespace WebsiteManagement.Infrastructure.Persistence.EntityConfigurations
{
    public class CategoryEntityConfigurations : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> categoryConfiguration)
        {
            categoryConfiguration.ToTable("categories");

            categoryConfiguration.HasKey(c => c.Id);
            categoryConfiguration.Property(c => c.Value).IsRequired();
        }
    }
}
