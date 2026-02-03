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
        public InvoiceItemConfig(string? tableName = "InvoiceItem") : base(tableName) { }
        public override void Configure(EntityTypeBuilder<InvoiceItem> builder) 
        {
            base.Configure(builder);

            builder.HasOne(x => x.Invoice)
                .WithMany()
                .HasForeignKey(f => f.InvoiceId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Medicine)
                .WithMany()
                .HasForeignKey(f => f.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            //.UsingEntity(j => j.ToTable("InvoiceMedicines"));
        }
    }
}
