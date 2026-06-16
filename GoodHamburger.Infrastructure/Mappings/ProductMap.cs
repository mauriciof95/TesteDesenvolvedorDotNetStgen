using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using GoodHamburger.Domain.Entities;

namespace GoodHamburger.Infrastructure.Mappings;

internal class ProductMap : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("product");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Name)
            .HasColumnName("name");
        builder.Property(x => x.Price)
            .HasColumnName("price");
        builder.Property(x => x.ProductType)
            .HasColumnName("product_type");

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at");
        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at");
        builder.Property(x => x.DeletedAt)
            .HasColumnName("deleted_at");

        builder.HasQueryFilter(x => x.DeletedAt == null);
    }
}