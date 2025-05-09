using LoanTrack.Application.Common.CQRS;

namespace LoanTrack.Application.Loans.Queries.Installments.GetNextInstallment;

public record GetNextInstallmentQuery(Guid LoanId): IQuery<GetInstallmentResponse>;
