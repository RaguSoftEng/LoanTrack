using LoanTrack.Application.Common.CQRS;

namespace LoanTrack.Application.Loans.Queries.Installments.GetInstallment;

public record GetInstallmentByIdQuery(Guid Id) : IQuery<GetInstallmentResponse>;
