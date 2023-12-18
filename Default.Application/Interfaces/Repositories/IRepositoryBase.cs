using Default.Domain.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Default.Application.Interfaces.Repositories
{
    //public interface IRepositoryBase<T> where T : BaseEntity
    //{
    //    T GetById(int id);

    //    DbSet<T> Table { get; }
    //    void Add(T entity);
    //    void Update(T entity);
    //    void Delete(T entity);

    //    void AddRange(IEnumerable<T> entities);
    //    void RemoveRange(IEnumerable<T> entities);

    //    IQueryable<T> All();
    //    IQueryable<T> Where(Expression<Func<T, bool>> where);
    //    IQueryable<T> OrderBy<TKey>(Expression<Func<T, TKey>> orderBy, bool isDesc);
    //}

    public interface IRepositoryBase
    {
        Task<TEntity> GetByIdAsync<TEntity>(Guid id) where TEntity : class;

        IEnumerable<TEntity> ListAll<TEntity>() where TEntity : class;

        Task<List<TEntity>> ListAllAsync<TEntity>() where TEntity : class;

        Task<List<TEntity>> WhereAsync<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class;

        List<TEntity> Where<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class;

        Task<TEntity> AddAsync<TEntity>(TEntity entity) where TEntity : class;

        Task<List<TEntity>> AddRangeAsync<TEntity>(List<TEntity> entity) where TEntity : class;

        void Update<TEntity>(TEntity entity) where TEntity : class;

        void UpdateManyAsync<TEntity>(List<TEntity> entities) where TEntity : class;

        void Delete<TEntity>(TEntity entity) where TEntity : class;

        void DeleteMany<TEntity>(List<TEntity> entities) where TEntity : class;

        IQueryable<TEntity> AsQueryable<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class;

        Task<TEntity> FistOrDefaultAsync<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class;

        TEntity FistOrDefault<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class;
    }
}
