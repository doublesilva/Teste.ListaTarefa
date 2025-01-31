namespace Teste.ListaTarefa.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellation);
        Task<T> GetByIdAsync(int id, CancellationToken cancellation);
        Task AddAsync(T entity, CancellationToken cancellation);
        Task UpdateAsync(T entity, CancellationToken cancellation);
        Task DeleteAsync(int id, CancellationToken cancellation);
    }
}
