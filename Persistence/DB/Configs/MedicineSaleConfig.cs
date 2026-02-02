using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace Persistence.DB.Configs
{
    internal class MedicineSaleConfig : BaseEntityEFConfig<MedicineSale>
    {
        public MedicineSaleConfig(string? tableName = "MedicineSale") : base(tableName) { }
        public override void Configure(EntityTypeBuilder<MedicineSale> builder) 
        {
            base.Configure(builder);

            builder.HasOne(u => u.User)
                .WithMany()
                .HasForeignKey(f => f.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(u => u.Medicine)
                .WithMany()
                .HasForeignKey(f => f.MedicineId)
                .OnDelete(DeleteBehavior.Restrict);
            //.UsingEntity(j => j.ToTable("UserMedicines"));
        }
    }
}
