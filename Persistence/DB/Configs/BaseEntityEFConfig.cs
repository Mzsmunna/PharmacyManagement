using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.DB.Configs
{
    internal class BaseEntityEFConfig<T>(string? tableName) : IEntityTypeConfiguration<T> where T : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            if (tableName is not null)
            {
                tableName = string.IsNullOrEmpty(tableName)
                    ? typeof(T).Name
                    : tableName;
                builder.ToTable(tableName);
            }
                
            builder.HasKey(u => u.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();
            //builder.Property(x => x.Id).ValueGeneratedOnAdd().UseIdentityColumn();
            //builder.HasQueryFilter(e => !e.IsDeleted);
            builder.HasQueryFilter("SoftDeleteFilter", e => !e.IsDeleted);
        }
    }
}
