using System.Linq.Expressions;

namespace LoanTrack.Domain.Common;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> IsExistAsync(Expression<Func<T, bool>>? expression = null, CancellationToken cancellationToken = default);
}
