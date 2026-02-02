using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.DB.Configs
{
    internal class CategoryConfig : BaseEntityEFConfig<Category>
    {
        public CategoryConfig(string? tableName = "User") : base(tableName) { }
        //public override void Configure(EntityTypeBuilder<Category> builder) { }
    }
}
