using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.DB.Configs
{
    internal class MedicineConfig : BaseEntityEFConfig<Medicine>
    {
        public MedicineConfig(string? tableName = "Medicine") : base(tableName) { }
        //public override void Configure(EntityTypeBuilder<Medicine> builder) 
        //{
        //    base.Configure(builder);
        //}
    }
}
