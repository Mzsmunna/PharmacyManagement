using Application.Abstractions;
using Domain.Entities;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.DB.Configs;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Text;

namespace Persistence.DB.Context
{
    /// <summary>
    /// dotnet ef migrations add ModTables --project Persistence --startup-project WebApi --output-dir DB/Migrations --context AppDBContext
    /// dotnet ef database update ModTables --project Persistence --startup-project WebApi --context AppDBContext
    /// </summary>
    public class AppDBContext : DbContext, IAppDBContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            //optionsBuilder.LogTo(Console.WriteLine);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDBContext).Assembly);
            //modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);          
            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.ApplyConfiguration(new MedicineConfig());
            modelBuilder.ApplyConfiguration(new MedicineBatchConfig());
            modelBuilder.ApplyConfiguration(new DetailOverviewConfig());
            modelBuilder.ApplyConfiguration(new InvoiceConfig());
            modelBuilder.ApplyConfiguration(new InvoiceItemConfig());

            //modelBuilder.Entity<User>().HasQueryFilter(b => !b.IsDeleted);
            modelBuilder.Entity<User>().HasQueryFilter("SoftDeleteFilter", e => !e.IsDeleted);
            modelBuilder.Entity<Count>(insu => { insu.HasNoKey(); });
            modelBuilder.Entity<Count>().ToTable("Count", t => t.ExcludeFromMigrations());
            modelBuilder.Entity<TotalCount>(insu => { insu.HasNoKey(); });
            modelBuilder.Entity<TotalCount>().ToTable("TotalCount", t => t.ExcludeFromMigrations());
        }

        public override int SaveChanges()
        {
            ModifyDateTime();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ModifyDateTime();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ModifyDateTime()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));
            foreach (var entry in entries)
            {
                var entity = (BaseEntity)entry.Entity;
                entity.ModifiedAt = DateTime.UtcNow;
                if (entry.State == EntityState.Added) entity.CreatedAt = DateTime.UtcNow;
            }
        }

        #region table_dbsets
        public virtual DbSet<Count> Counts { get; set; }
        public virtual DbSet<TotalCount> TotalCounts { get; set; }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Medicine> Medicines { get; set; }
        public virtual DbSet<MedicineBatch> MedicineBatches { get; set; }
        public virtual DbSet<DetailOverview> DetailOverviews { get; set; }        
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<InvoiceItem> InvoiceItems { get; set; }       
        
        #endregion
    }
}
