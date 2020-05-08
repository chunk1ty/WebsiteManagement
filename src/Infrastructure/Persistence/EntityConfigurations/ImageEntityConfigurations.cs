using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebsiteManagement.Domain;

namespace WebsiteManagement.Infrastructure.Persistence.EntityConfigurations
{
    public class ImageEntityConfigurations : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> imageConfiguration)
        {
            imageConfiguration.ToTable("images");

            imageConfiguration.HasKey(i => i.Id);
            imageConfiguration.Property(i => i.Blob).IsRequired();
        }
    }
}
