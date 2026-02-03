using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.DB.Configs
{
    internal class DetailOverviewConfig : BaseEntityEFConfig<DetailOverview>
    {
        public DetailOverviewConfig(string? tableName = "DetailOverviews") : base(tableName) { }
        //public override void Configure(EntityTypeBuilder<DetailOverview> builder) { }
    }
}
