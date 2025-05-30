﻿using Ecom.Core.Entities.Product;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Data.Config
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Category> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(30);
            builder.Property(x => x.Id).IsRequired();
            builder.HasData(
                new Category
            {
                Id = 1,
                Name = "Flowers",
                Description = "Plants"

            },
                new Category
                {
                    Id = 2,
                    Name = "Clothing",
                    Description = "Apparel and garments"
                },
                new Category
                {
                    Id = 3,
                    Name = "Electronics",
                    Description = "Electronic devices and gadgets"
                }

            );


        }
    }
}
