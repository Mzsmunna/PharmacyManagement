using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.DB.Configs
{
    internal class MedicineSaleConfig : BaseEntityEFConfig<MedicineSale>
    {
        public MedicineSaleConfig(string? tableName = "MedicineSale") : base(tableName) { }
        //public override void Configure(EntityTypeBuilder<MedicineSale> builder) { }
    }
}
