using LoanTrack.Domain.Common;

namespace LoanTrack.Domain.ListValues;

public interface IListValueRepository : IGenericRepository<ListValue>
{
    //Task<bool> IsListValueExistAsync(Guid parentId, string listType, List<string> values, CancellationToken cancellationToken = default);
}
