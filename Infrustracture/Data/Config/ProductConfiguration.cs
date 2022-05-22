using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrustracture.Data.Config
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Id).IsRequired();
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Desc).IsRequired().HasMaxLength(180);
            builder.Property(p => p.RegDT).IsRequired().HasMaxLength(20);
            builder.Property(p => p.UpDT).IsRequired().HasMaxLength(20);
            builder.Property(p => p.IsActive).HasColumnType("bit");

            //Relations
            builder.HasOne(b => b.ProductBrand).WithMany().HasForeignKey(p => p.ProductBrandId);
            builder.HasOne(t => t.ProductType).WithMany().HasForeignKey(p => p.ProductTypeId);
            //builder.HasOne(pic => pic.ProductPic).WithMany().HasForeignKey(p => p.ProductPicId);
            //builder.HasOne(prc => prc.ProductPrice).WithMany().HasForeignKey(p => p.ProductPriceId);
        }
    }
}