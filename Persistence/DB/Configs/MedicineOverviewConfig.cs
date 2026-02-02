using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.DB.Configs
{
    internal class MedicineOverviewConfig : BaseEntityEFConfig<DetailOverview>
    {
        public MedicineOverviewConfig(string? tableName = "MedicineOverview") : base(tableName) { }
        //public override void Configure(EntityTypeBuilder<MedicineOverview> builder) { }
    }
}
