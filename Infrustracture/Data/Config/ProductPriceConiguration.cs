using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrustracture.Data.Config
{
    public class ProductPriceConfiguration : IEntityTypeConfiguration<ProductPrice>
    {
        public void Configure(EntityTypeBuilder<ProductPrice> builder)
        {
            builder.Property(p => p.ProductId).IsRequired();
            builder.Property(p => p.Price).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(p => p.RegDT).IsRequired().HasMaxLength(20);
            builder.Property(p => p.IsActive).IsRequired().HasColumnType("bit");
            //Relations
            builder.HasOne(p => p.Product).WithMany().HasForeignKey(p => p.ProductId);

        }
    }
}