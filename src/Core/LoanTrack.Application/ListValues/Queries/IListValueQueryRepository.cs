using LoanTrack.Application.Common.DTOs;

namespace LoanTrack.Application.ListValues.Queries;

public interface IListValueQueryRepository
{
    Task<List<ListValueResponse>> GetValuesByListAsync(string listName, Guid parentId, CancellationToken cancellationToken = default);
}
