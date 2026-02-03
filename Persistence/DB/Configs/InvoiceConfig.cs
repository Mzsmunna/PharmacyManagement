using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace Persistence.DB.Configs
{
    internal class InvoiceConfig : BaseEntityEFConfig<Invoice>
    {
        public InvoiceConfig(string? tableName = "Invoice") : base(tableName) { }
        public override void Configure(EntityTypeBuilder<Invoice> builder) 
        {
            base.Configure(builder);

            builder.HasOne(x => x.User)
                .WithMany(f => f.Invoices)
                .HasForeignKey(f => f.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Items)
                .WithOne(f => f.Invoice)
                .HasForeignKey(f => f.InvoiceId)
                .OnDelete(DeleteBehavior.Restrict);

            //.UsingEntity(j => j.ToTable("UserMedicines"));
        }
    }
}
