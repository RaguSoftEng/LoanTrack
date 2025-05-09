using LoanTrack.Application.Common.CQRS;

namespace LoanTrack.Application.Loans.Queries.Installments.GetNextInstallments;

public record GetNextInstallmentsQuery(Guid? CenterId, Guid? GroupId): IQuery<IReadOnlyCollection<InstallmentsListResponse>>;
