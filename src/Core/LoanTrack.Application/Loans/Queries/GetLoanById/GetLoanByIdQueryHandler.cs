using LoanTrack.Application.Common.CQRS;
using LoanTrack.Domain.Common;

namespace LoanTrack.Application.Loans.Queries.GetLoanById;

public class GetLoanByIdQueryHandler(
    ILoanQueryRepository repository
) : IQueryHandler<GetLoanByIdQuery, GetLoanByIdResponse>
{
    public async Task<Result<GetLoanByIdResponse>> Handle(GetLoanByIdQuery request, CancellationToken cancellationToken)
    {
        var response = await repository.GetLoanByIdAsync(request.LoanId, cancellationToken);
        if(response == null) Result.Failure(Error.Failure("404", $"No loan found with id: {request.LoanId}"));
        return response;
    }
}
