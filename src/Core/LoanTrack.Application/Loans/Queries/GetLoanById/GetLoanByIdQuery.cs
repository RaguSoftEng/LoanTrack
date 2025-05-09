using LoanTrack.Application.Common.CQRS;

namespace LoanTrack.Application.Loans.Queries.GetLoanById;

public record GetLoanByIdQuery(Guid LoanId): IQuery<GetLoanByIdResponse>;
