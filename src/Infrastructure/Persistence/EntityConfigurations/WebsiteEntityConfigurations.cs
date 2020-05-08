using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebsiteManagement.Domain;

namespace WebsiteManagement.Infrastructure.Persistence.EntityConfigurations
{
    public class WebsiteEntityConfigurations : IEntityTypeConfiguration<Website>
    {
        public void Configure(EntityTypeBuilder<Website> websiteConfiguration)
        {
            RelationalEntityTypeBuilderExtensions.ToTable((EntityTypeBuilder) websiteConfiguration, "websites");

            websiteConfiguration.HasKey(w => w.Id);
            websiteConfiguration.HasOne(w => w.Image);
            websiteConfiguration.HasMany(x => x.Categories);

            websiteConfiguration.Property(w => w.Name).IsRequired();
            websiteConfiguration.HasIndex(w => w.Url).IsUnique();
            websiteConfiguration.Property(w => w.Email).IsRequired();
            websiteConfiguration.Property(w => w.Password).IsRequired();
        }
    }
}
