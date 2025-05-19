using Ecom.Core.Entities.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Data.Config
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x=>x.Description).IsRequired();
            builder.Property(x=>x.NewPrice).HasColumnType("decimal(18,2)");
            builder.HasData(
                new Product
                {
                Id = 1,
                Name = "Rose",
                Description = "Red Rose",
                NewPrice = 100,
                CategoryId = 1
            },
                new Product
                {
                    Id = 2,
                    Name = "Men's T-Shirt",
                    Description = "100% cotton, available in multiple sizes",
                    NewPrice = 29.99m,
                    CategoryId = 2
                },
                new Product
                {
                    Id = 3,
                    Name = "Wireless Headphones",
                    Description = "Noise-cancelling Bluetooth headphones",
                    NewPrice = 89.99m,
                    CategoryId = 3
                }


            );
        }
    }
}
