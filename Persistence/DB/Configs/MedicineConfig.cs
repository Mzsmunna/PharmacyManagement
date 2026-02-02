using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.DB.Configs
{
    internal class MedicineConfig : BaseEntityEFConfig<Medicine>
    {
        public MedicineConfig(string? tableName = "Medicine") : base(tableName) { }
        //public override void Configure(EntityTypeBuilder<Medicine> builder) { }
    }
}
