using LoanTrack.Application.Common.CQRS;

namespace LoanTrack.Application.Loans.Queries.Installments.GetInstallments;

public record GetInstallmentsQuery(Guid LoanId) : IQuery<IReadOnlyCollection<InstallmentsListResponse>>;
