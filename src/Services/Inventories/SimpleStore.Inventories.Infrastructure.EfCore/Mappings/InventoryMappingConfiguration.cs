﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleStore.Inventories.Domain.Models;

namespace SimpleStore.Inventories.Infrastructure.EfCore.Mappings
{
    public class InventoryMappingConfiguration : IEntityTypeConfiguration<Domain.Models.Inventory>
    {
        #region Implementation of IEntityTypeConfiguration<Inventory>

        public void Configure(EntityTypeBuilder<Domain.Models.Inventory> builder)
        {
            builder.HasKey(x => x.InventoryId);

            builder
                .Property(x => x.InventoryId)
                .HasField("Id")
                .HasColumnName("Id")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasConversion(x => x.Id, id => (InventoryId)id);

            builder
                .HasMany(x => x.Products)
                .WithOne()
                .HasForeignKey(x => x.InventoryId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }

        #endregion
    }
}
