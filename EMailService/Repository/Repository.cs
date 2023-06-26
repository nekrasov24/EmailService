using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EMailService.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();

        }

        public async Task<IQueryable<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null)
        {


            var result = _dbSet.AsQueryable();

            if (includes != null)
                result = includes(result);

            return await Task.FromResult(predicate != null ? result.Where(predicate) : result);
        }

        public async Task<T> FindAsync()
        {
            return await _dbSet.FirstAsync();
        }

        public async Task AddAsync(T obj)
        {
            _dbSet.Add(obj);
            await SaveChangeAsync();
        }

        public async Task SaveChangeAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
