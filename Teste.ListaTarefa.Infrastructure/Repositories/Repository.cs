using Microsoft.EntityFrameworkCore;
using TaskListAPI.Infrastructure.Data;
using Teste.ListaTarefa.Domain.Interfaces;

namespace Teste.ListaTarefa.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly TaskDbContext _context;

        public Repository(TaskDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellation)
        {
            return await _context.Set<T>().ToListAsync(cancellation);
        }

        public async Task<T> GetByIdAsync(int id, CancellationToken cancellation) => 
            await _context.Set<T>().FindAsync(id, cancellation);

        public async Task AddAsync(T entity, CancellationToken cancellation)
        {
            await _context.Set<T>().AddAsync(entity, cancellation);
            await _context.SaveChangesAsync(cancellation);
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellation)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync(cancellation);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellation)
        {
            var entity = await _context.Set<T>().FindAsync(id, cancellation);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync(cancellation);
            }
        }
    }
}
