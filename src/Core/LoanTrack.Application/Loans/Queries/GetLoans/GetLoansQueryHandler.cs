using LoanTrack.Application.Common.CQRS;
using LoanTrack.Application.Common.DTOs;
using LoanTrack.Domain.Common;

namespace LoanTrack.Application.Loans.Queries.GetLoans;

public class GetLoansQueryHandler(
    ILoanQueryRepository repository
): IQueryHandler<GetLoansQuery, PaginatedResult<GetLoansResponse>>
{
    public async Task<Result<PaginatedResult<GetLoansResponse>>> Handle(GetLoansQuery request,
        CancellationToken cancellationToken)
    {
        var response = await repository.GetLoansByFilterAsync(
            request.ToBaseQuery(),
            request.CenterId,
            request.GroupId,
            request.Nic,
            cancellationToken
        );
        return Result.Success(response);
    }
}
