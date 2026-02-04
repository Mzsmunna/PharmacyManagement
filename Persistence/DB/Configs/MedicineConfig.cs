using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace Persistence.DB.Configs
{
    internal class MedicineConfig : BaseEntityEFConfig<Medicine>
    {
        public MedicineConfig(string? tableName = "Medicines") : base(tableName) { }
        public override void Configure(EntityTypeBuilder<Medicine> builder)
        {
            base.Configure(builder);

            //builder.HasIndex(pt => pt.SKU).IsUnique();

            builder.HasMany(p => p.Details)
                .WithMany(t => t.Medicines)
                .UsingEntity(j => j.ToTable("MedicineDetails")); // custom join table name

            builder.HasMany(p => p.Batches)
                .WithOne(t => t.Medicine)
                .HasForeignKey(t => t.MedicineId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
