using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shopping.Core.Domains;

namespace Shopping.Infrastructure.Persistence.Shopping.Configurations
{
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.Property(e => e.Id)
                .HasColumnName("Id")
                .ValueGeneratedNever();
            
            builder.HasKey(e => e.Id);
            
            builder.HasIndex(e => new {e.OrderId, e.ProductId}).IsUnique();

            builder.ToTable("Order Details");

            builder.Property(e => e.OrderId).HasColumnName("OrderID");

            builder.Property(e => e.ProductId).HasColumnName("ProductID");

            builder.Property(e => e.Quantity).HasDefaultValueSql("((1))");

            builder.Property(e => e.UnitPrice).HasColumnType("money");

            builder.HasOne(d => d.Order)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Details_Orders");

            builder.HasOne(d => d.Product)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Details_Products");
        }
    }
}