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
            builder.ToTable(tableName ?? typeof(T).Name);
            builder.HasKey(u => u.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();
            //builder.Property(x => x.Id).ValueGeneratedOnAdd().UseIdentityColumn();
            //builder.HasQueryFilter(e => !e.IsDeleted);
            builder.HasQueryFilter("SoftDeleteFilter", e => !e.IsDeleted);
        }
    }
}
