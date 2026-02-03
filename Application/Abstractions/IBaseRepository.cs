using Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Application.Abstractions
{
    public interface IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken token = default);
        Task<IEnumerable<TEntity>> GetAllAsNoTrackAsync(CancellationToken token = default);
        Task<TEntity?> GetByIdAsync(string id, CancellationToken token = default);
        Task<TEntity?> GetByIdAsNoTrackAsync(string id, CancellationToken token = default);
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default);
        Task<IEnumerable<TEntity>> FindAsNoTrackAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default);
        Task<List<TEntity>> ExecuteSPAsync(string spName, List<SqlParameter> sqlParams);
        Task<List<TModel>> ExecuteAnySPAsync<TModel>(string spName, List<SqlParameter> sqlParams) where TModel : class, new();
        Task<List<TEntity>> ExecuteRawSqlAsync(string sql, List<SqlParameter>? sqlParams);
        Task<List<TModel>> ExecuteAnyRawSqlAsync<TModel>(string sql, List<SqlParameter> sqlParams) where TModel : class, new();
        Task<int> UpdateDbRawSqlAsync(string sql, List<SqlParameter> sqlParams);
        Task AddAsync(TEntity entity, CancellationToken token = default);
        Task AddRangeAsync(List<TEntity> entityList, CancellationToken token = default);
        Task SaveChangesAsync(CancellationToken token = default);
    }
}
