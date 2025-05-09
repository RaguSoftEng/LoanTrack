using LoanTrack.Application.Common.DTOs;
using LoanTrack.Application.LoanSchemes.Queries.GetById;
using LoanTrack.Application.LoanSchemes.Queries.GetList;
using LoanTrack.Domain.LoanSchemes;

namespace LoanTrack.Application.LoanSchemes.Queries;

public interface ILoanSchemeQueryRepository
{
    Task<LoanScheme> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<LoanScheme>> GetAllAsync( CancellationToken cancellationToken = default);
    Task<List<LoanSchemeListResponse>> GetListAsync( CancellationToken cancellationToken = default);
    Task<List<ListValueResponse>> GetListValuesAsync( CancellationToken cancellationToken = default);
}
