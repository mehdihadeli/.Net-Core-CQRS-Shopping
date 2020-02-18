using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shopping.Core.Domains;

namespace Shopping.Infrastructure.Persistence.Shopping.Configurations
{
    public class RegionConfiguration : IEntityTypeConfiguration<Region>
    {
        public void Configure(EntityTypeBuilder<Region> builder)
        {
            builder.Property(e => e.Id)
                .HasColumnName("Id")
                .ValueGeneratedNever();
            builder.HasKey(e => e.Id);
            
            builder.Property(e => e.RegionDescription)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
