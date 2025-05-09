using LoanTrack.Application.Common.CQRS;
using LoanTrack.Domain.Common;

namespace LoanTrack.Application.LoanSchemes.Queries.GetById;

public class GetLoanSchemeByIdHandler(
    ILoanSchemeQueryRepository repository
) : IQueryHandler<GetLoanSchemeByIdQuery, LoanSchemeResponse>
{
    public async Task<Result<LoanSchemeResponse>> Handle(GetLoanSchemeByIdQuery request, CancellationToken cancellationToken)
    {
        var response = await repository.GetByIdAsync(request.Id, cancellationToken);
        var scheme = LoanSchemeResponse.FromEntity(response);
        return scheme;
    }
}
