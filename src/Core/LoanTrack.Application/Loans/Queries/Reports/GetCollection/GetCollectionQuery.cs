using LoanTrack.Application.Common.CQRS;

namespace LoanTrack.Application.Loans.Queries.Reports.GetCollection;

public record GetCollectionQuery(DateOnly DateFrom, DateOnly DateTo) : IQuery<CollectionResponse>;
