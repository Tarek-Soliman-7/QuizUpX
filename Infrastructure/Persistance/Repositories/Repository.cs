using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Persistance.Data;

namespace Persistance.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;


        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }


        public virtual async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }


        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }


        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }


        public virtual void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }


        public virtual void Update(T entity)
        {
            _dbSet.Update(entity);
        }
    }
}