using GoodHamburger.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoodHamburger.Infrastructure.Mappings;

internal class OrderItemMap : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("order_item");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();
        builder.Property(x => x.OrderId)
            .HasColumnName("order_id");
        builder.Property(x => x.ProductId)
            .HasColumnName("product_id");
        builder.Property(x => x.ProductType)
            .HasColumnName("product_type");
        builder.Property(x => x.CurrentPrice)
            .HasColumnName("current_price"); ;
        builder.Property(x => x.ProductName)
            .HasColumnName("product_name"); ;

        builder.HasOne(x => x.Order)
            .WithMany(x => x.OrderItems)
            .HasForeignKey(x => x.OrderId);
        builder.HasOne(x => x.Product)
            .WithMany()
            .HasForeignKey(x => x.ProductId);

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at");
        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at");
        builder.Property(x => x.DeletedAt)
            .HasColumnName("deleted_at");
    }
}
