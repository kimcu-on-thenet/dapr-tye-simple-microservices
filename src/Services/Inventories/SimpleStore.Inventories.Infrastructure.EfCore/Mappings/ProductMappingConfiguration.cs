﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleStore.Inventories.Domain.Models;

namespace SimpleStore.Inventories.Infrastructure.EfCore.Mappings
{
    public class ProductMappingConfiguration : IEntityTypeConfiguration<Product>
    {
        #region Implementation of IEntityTypeConfiguration<Product>

        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder
                .Property(x => x.Id)
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasConversion(x => x.Id, id => (ProductId)id);

            builder.Metadata
                .FindNavigation(nameof(Product.Inventories))
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            //builder
            //    .HasMany(x => x.Inventories)
            //    .WithOne(x => x.Product)
            //    .HasForeignKey(x => x.ProductId)
            //    .IsRequired()
            //    .OnDelete(DeleteBehavior.Cascade);
        }

        #endregion
    }
}
