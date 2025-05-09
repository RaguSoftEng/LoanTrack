using System.Linq.Expressions;
using LoanTrack.Domain.Common;
using LoanTrack.Persistence.Common.Database;
using LoanTrack.Persistence.Common.Extensions;
using Microsoft.EntityFrameworkCore;

namespace LoanTrack.Persistence.Common;

public class GenericRepository<T>(ApplicationDbContext context) : IGenericRepository<T> where T : BaseEntity
{
    protected readonly ApplicationDbContext Context = context;
    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        => await Context.Set<T>().AddAsync(entity, cancellationToken);

    public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        => await Context.Set<T>().AddRangeAsync(entities, cancellationToken);

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await Context.Set<T>().FirstOrDefaultAsync(x=>x.Id==id, cancellationToken);

    public Task<bool> IsExistAsync(
        Expression<Func<T, bool>>? expression = null,
        CancellationToken cancellationToken = default
    ) => Context.Set<T>().WhereIf(expression != null, expression!).AnyAsync(cancellationToken);
}
