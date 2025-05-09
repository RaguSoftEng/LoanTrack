using LoanTrack.Application.Common.CQRS;
using LoanTrack.Domain.Common;

namespace LoanTrack.Application.Loans.Queries.Reports.GetCollection;

public class GetCollectionQueryHandler(
    IInstallmentQueryRepository repository
): IQueryHandler<GetCollectionQuery, CollectionResponse>
{
    public Task<Result<CollectionResponse>> Handle(
        GetCollectionQuery request,
        CancellationToken cancellationToken
    ) => repository.GetCollectionAsync(request.DateFrom, request.DateTo, cancellationToken);
}
