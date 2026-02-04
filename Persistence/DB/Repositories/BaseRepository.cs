using Application.Abstractions;
using Application.Dtos;
using Application.Exceptions;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Persistence.DB.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly IAppDBContext dbContext;
        protected readonly DbSet<TEntity> entities;

        public BaseRepository(IAppDBContext context)
        {
            dbContext = context;
            entities = dbContext.Set<TEntity>();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken token = default)
        {
            return await entities.ToListAsync(token);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsNoTrackAsync(CancellationToken token = default)
        {
            return await entities.AsNoTracking().ToListAsync(token);
        }

        public virtual async Task<TEntity?> GetByIdAsync(string id, CancellationToken token = default)
        {
            return await entities.FindAsync(id, token);
        }

        public virtual async Task<TEntity?> GetByIdAsNoTrackAsync(string id, CancellationToken token = default)
        {
            //return await entities.Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync(token);
            return await entities.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, token);
        }

        public virtual async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default)
        {
            return await entities.Where(predicate).ToListAsync(token);
        }

        public virtual async Task<IEnumerable<TEntity>> FindAsNoTrackAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default)
        {
            return await entities.Where(predicate).AsNoTracking().ToListAsync(token);
        }

        public virtual async Task<List<TEntity>> ExecuteSPAsync(string spName, List<SqlParameter>? sqlParams)
        {
            if (string.IsNullOrEmpty(spName) || string.IsNullOrWhiteSpace(spName)) return [];
            if (sqlParams is null || sqlParams.Count <= 0)
            {
                var resultNoParams = await entities.FromSqlInterpolated($"exec {spName}").AsNoTracking().ToListAsync();
                return resultNoParams;
            }
            var allSqlParams = string.Join(",", sqlParams);
            var result = await entities.FromSqlInterpolated($"exec {spName} {allSqlParams}").AsNoTracking().ToListAsync();
            return result;
        }

        public virtual async Task<List<TModel>> ExecuteAnySPAsync<TModel>(string spName, List<SqlParameter>? sqlParams) where TModel : class, new()
        {
            if (string.IsNullOrEmpty(spName) || string.IsNullOrWhiteSpace(spName)) return [];
            if (sqlParams is null || sqlParams.Count <= 0)
            {
                var resultNoParams = await dbContext.Set<TModel>().FromSqlInterpolated($"exec {spName}").AsNoTracking().ToListAsync();
                return resultNoParams;
            }
            var allSqlParams = string.Join(",", sqlParams);
            var result = await dbContext.Set<TModel>().FromSqlInterpolated($"exec {spName} {allSqlParams}").AsNoTracking().ToListAsync();
            return result;
        }

        public virtual async Task<List<TEntity>> ExecuteRawSqlAsync(string sql, List<SqlParameter>? sqlParams)
        {
            if (string.IsNullOrEmpty(sql)) return [];
            if (sqlParams is null || sqlParams.Count <= 0)
            {
                var resultNoParams = await entities.FromSqlRaw(sql).AsNoTracking().ToListAsync();
                return resultNoParams;
            }
            var allSqlParams = string.Join(",", sqlParams);
            var result = await entities.FromSqlRaw(sql, sqlParams).AsNoTracking().ToListAsync();
            return result;
        }

        public virtual async Task<List<TModel>> ExecuteAnyRawSqlAsync<TModel>(string sql, List<SqlParameter> sqlParams) where TModel : class, new()
        {
            if (string.IsNullOrEmpty(sql)) return [];
            if (sqlParams is null || sqlParams.Count <= 0)
            {
                var resultNoParams = await dbContext.Set<TModel>().FromSqlRaw(sql).AsNoTracking().ToListAsync();
                return resultNoParams;
            }
            var allSqlParams = string.Join(",", sqlParams);
            var result = await dbContext.Set<TModel>().FromSqlRaw(sql, sqlParams).AsNoTracking().ToListAsync();
            return result;
        }

        public virtual async Task<int> UpdateDbRawSqlAsync(string sql, List<SqlParameter> sqlParams)
        {
            if (string.IsNullOrEmpty(sql)) return 0;
            var resultNoParams = await dbContext.Database.ExecuteSqlRawAsync(sql);
            return resultNoParams;
        }

        public virtual async Task<bool> AddAsync(TEntity entity, CancellationToken token = default)
        {
            try
            {
                await entities.AddAsync(entity, token);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public virtual async Task<bool> AddRangeAsync(List<TEntity> entityList, CancellationToken token = default)
        {
            try
            {
                await entities.AddRangeAsync(entityList, token);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public virtual async Task<bool> UpdateAsync(TEntity entity, CancellationToken token = default)
        {
            try
            {
                entities.Update(entity);
                await dbContext.SaveChangesAsync(token);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public virtual async Task<bool> DeleteByIdAsync(string id, CancellationToken token = default)
        {
            try
            {
                var result = await GetByIdAsync(id);
                if (result == null) throw new AppException(AppError.Missing(typeof(TEntity).Name + ".Missing", "Missing user id: " + id));
                result.IsDeleted = true;
                result.DeletedAt = DateTime.UtcNow;
                await dbContext.SaveChangesAsync(token);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public virtual async Task<bool> SaveChangesAsync(TEntity entity, CancellationToken token = default)
        { 
            try
            {
                entities.Attach(entity);
                dbContext.Entry(entities).State = EntityState.Modified;
                await dbContext.SaveChangesAsync(token);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public virtual async Task<bool> SaveChangesAsync(CancellationToken token = default)
        {
            try
            {
                await dbContext.SaveChangesAsync(token);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
