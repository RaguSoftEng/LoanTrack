using System.Runtime.Intrinsics.X86;
using LoanTrack.Application.Common.DTOs;
using LoanTrack.Application.ListValues.Queries;
using LoanTrack.Persistence.Common.Database;
using Microsoft.EntityFrameworkCore;

namespace LoanTrack.Persistence.ListValues;

public class ListValueQueryRepository(ApplicationDbContext context) : IListValueQueryRepository
{
    public async Task<List<ListValueResponse>> GetValuesByListAsync(string listName, Guid parentId, CancellationToken cancellationToken = default)
     => await context.ListValues.Where(x=>x.ListType == listName && (parentId == Guid.Empty || x.ParentId == parentId))
         .Select(x=> new ListValueResponse(x.Id, x.Description, false))
         .AsNoTracking()
         .ToListAsync(cancellationToken);
}
