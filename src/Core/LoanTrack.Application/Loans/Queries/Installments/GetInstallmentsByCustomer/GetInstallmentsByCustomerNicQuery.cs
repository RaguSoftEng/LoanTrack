using LoanTrack.Application.Common.CQRS;

namespace LoanTrack.Application.Loans.Queries.Installments.GetInstallmentsByCustomer;

public record GetInstallmentsByCustomerNicQuery(string Nic) : IQuery<IReadOnlyCollection<InstallmentsListResponse>>;
