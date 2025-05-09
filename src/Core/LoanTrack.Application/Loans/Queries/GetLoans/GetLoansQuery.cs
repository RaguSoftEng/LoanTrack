using LoanTrack.Application.Common.CQRS;
using LoanTrack.Application.Common.DTOs;

namespace LoanTrack.Application.Loans.Queries.GetLoans;

public record GetLoansQuery : QueryParameters, IQuery<PaginatedResult<GetLoansResponse>>
{
    public Guid CenterId { get; set; }
    public Guid GroupId { get; set; }
    public string Nic { get; set; }
}
