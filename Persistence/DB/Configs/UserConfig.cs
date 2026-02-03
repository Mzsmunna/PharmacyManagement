using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.DB.Configs
{
    internal class UserConfig : BaseEntityEFConfig<User>
    {
        public UserConfig(string? tableName = "Users") : base(tableName) { }
        //public override void Configure(EntityTypeBuilder<Category> builder) { }
    }
}
