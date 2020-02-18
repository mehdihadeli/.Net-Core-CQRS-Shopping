using System;
using Common.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Shopping.Infrastructure.Persistence.Identity.Configurations
{
    public class RefreshTokenConfiguration: IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> modelBuilder)
        {
            modelBuilder.ToTable("RefreshTokens");

            modelBuilder.HasKey(x => x.Id);
            modelBuilder.Property(e => e.Id).IsRequired()
                .ValueGeneratedOnAdd();

            modelBuilder.Property(x => x.UserId);

            modelBuilder.Property(s => s.Token)
                .IsRequired();

            modelBuilder.Property(s => s.RevokedAt)
                .IsRequired(false);

            modelBuilder.Property(s => s.CreatedAt)
                .IsRequired()
                .HasDefaultValue(DateTime.Now);

            modelBuilder.Property(s => s.Expires)
                .IsRequired();
        }
    }
}

