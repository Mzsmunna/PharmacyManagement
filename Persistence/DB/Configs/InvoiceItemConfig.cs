using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace Persistence.DB.Configs
{
    internal class InvoiceItemConfig : BaseEntityEFConfig<InvoiceItem>
    {
        public InvoiceItemConfig(string? tableName = "InvoiceItems") : base(tableName) { }
        public override void Configure(EntityTypeBuilder<InvoiceItem> builder) 
        {
            base.Configure(builder);

            //builder.HasKey(pt => new { pt.InvoiceId, pt.MedicineId });
            //builder.HasIndex(pt => new { pt.InvoiceId, pt.MedicineId })
            builder.HasIndex(pt => new { pt.InvoiceId, pt.MedicineBatchId })
                .IsUnique();

            builder.HasOne(x => x.Invoice)
                .WithMany(f => f.InvoiceItems)
                .HasForeignKey(f => f.InvoiceId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Batch)
                .WithMany(f => f.InvoiceItems)
                .HasForeignKey(f => f.MedicineBatchId)
                .OnDelete(DeleteBehavior.Restrict);

            //builder.HasOne(x => x.Medicine)
            //    .WithMany(f => f.InvoiceItems)
            //    .HasForeignKey(f => f.MedicineId)
            //    .OnDelete(DeleteBehavior.Restrict);

            //.UsingEntity(j => j.ToTable("InvoiceItems"));
        }
    }
}
