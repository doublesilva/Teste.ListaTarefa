using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Teste.ListaTarefa.Domain.Interfaces;


namespace Teste.ListaTarefa.Infrastructure.Repositories
{

    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly TaskDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(TaskDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T> GetByIdAsync(int id, CancellationToken cancellation = default(CancellationToken), params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.SingleOrDefaultAsync(e => EF.Property<int>(e, "Id") == id, cancellation);
        }

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellation = default(CancellationToken), params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync(cancellation);
        }


        public async Task AddAsync(T entity, CancellationToken cancellation)
        {
            await _dbSet.AddAsync(entity, cancellation);
            await _context.SaveChangesAsync(cancellation);
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellation)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync(cancellation);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellation)
        {
            var entity = await _dbSet.FindAsync(id, cancellation);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync(cancellation);
            }
        }
    }

}
