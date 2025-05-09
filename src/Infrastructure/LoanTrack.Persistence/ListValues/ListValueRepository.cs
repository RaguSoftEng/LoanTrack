using LoanTrack.Domain.ListValues;
using LoanTrack.Persistence.Common;
using LoanTrack.Persistence.Common.Database;
using Microsoft.EntityFrameworkCore;

namespace LoanTrack.Persistence.ListValues;

public class ListValueRepository(ApplicationDbContext context) : GenericRepository<ListValue>(context), IListValueRepository
{
    public async Task<IReadOnlyCollection<ListValue>> GetValuesByListAsync(
        string listName,
        CancellationToken cancellationToken = default
    ) => await Context.ListValues.Where(x=>x.ListType == listName).ToListAsync(cancellationToken);

    /*public async Task<bool> IsListValueExistAsync(Guid parentId, string listType, List<string> values, CancellationToken cancellationToken = default)
    => await Context.ListValues.Where()*/
}
