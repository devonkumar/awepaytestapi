using AwePay.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AwePay.Generics
{
    public interface IRepository<T> where T : class
    {
        Task<T> Get(int id);
        Task<List<T>> GetAll();

        bool Any(Expression<Func<T, bool>> predicate);

        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        T Single(Expression<Func<T, bool>> predicate);
        Task<T> SingleAsync(Expression<Func<T, bool>> predicate);

        void Add(T entity);

        void AddAsync(T entity);

        void Update(T entity);
        void AddRange(IEnumerable<T> entities);

        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);

        void Delete(object Id);
    }

    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbCtx Context;
        private DbSet<T> _entities;

        public const int defPageNum = 1;
        public const int defPageSize = 50;

        public Repository(DbCtx context)
        {
            Context = context;
            _entities = Context.Set<T>();
        }

        public async Task<T> Get(int id)
        {
            return await _entities.FindAsync(id);
        }

        public async Task<T> Get(long id)
        {
            return await _entities.FindAsync(id);
        }

        public bool Any(Expression<Func<T, bool>> predicate)
        {
            return _entities.Any(predicate);
        }

        public Task<List<T>> GetAll()
        {
            return _entities.ToListAsync();
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _entities.Where(predicate).ToList();
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _entities.Where(predicate).ToListAsync();
        }

        public void Add(T entity)
        {
            _entities.Add(entity);
        }
        public async void AddAsync(T entity)
        {
            await _entities.AddAsync(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            _entities.AddRange(entities);
        }

        public void Remove(T entity)
        {
            _entities.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _entities.RemoveRange(entities);
        }


        public async Task<T> SingleAsync(Expression<Func<T, bool>> predicate)
        {
            return await _entities.SingleOrDefaultAsync(predicate);
        }

        public T Single(Expression<Func<T, bool>> predicate)
        {
            return _entities.SingleOrDefault(predicate);
        }

        public void Update(T entity)
        {
            _entities.Update(entity);

        }

        public async void Delete(object Id)
        {
            T entity = await _entities.FindAsync(Id);
            Delete(entity);
        }
    }
}
