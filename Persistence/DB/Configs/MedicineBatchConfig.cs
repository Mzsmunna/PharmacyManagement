using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace Persistence.DB.Configs
{
    internal class MedicineBatchConfig : BaseEntityEFConfig<MedicineBatch>
    {
        public MedicineBatchConfig(string? tableName = "MedicineBatches") : base(tableName) { }
        public override void Configure(EntityTypeBuilder<MedicineBatch> builder) 
        {
            base.Configure(builder);

            builder.HasIndex(pt => new { pt.No, pt.MedicineId })
                    .IsUnique();

            builder.HasOne(x => x.Medicine)
                .WithMany(f => f.Batches)
                .HasForeignKey(f => f.MedicineId)
                .OnDelete(DeleteBehavior.Restrict);

            //.UsingEntity(j => j.ToTable("UserMedicines"));
        }
    }
}
