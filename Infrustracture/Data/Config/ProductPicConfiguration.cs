using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrustracture.Data.Config
{
    public class ProductPicConfiguration : IEntityTypeConfiguration<ProductPic>
    {
        public void Configure(EntityTypeBuilder<ProductPic> builder)
        {
            builder.Property(p => p.ProductId).IsRequired();
            builder.Property(p => p.Url).IsRequired().HasMaxLength(100);
            builder.Property(p => p.RegDT).IsRequired().HasMaxLength(20);
            builder.Property(p => p.IsActive).HasColumnType("bit");
            builder.Property(p => p.RegDT).HasMaxLength(180);
            //Relations
            builder.HasOne(p => p.Product).WithMany().HasForeignKey(p => p.ProductId);
        }
    }
}