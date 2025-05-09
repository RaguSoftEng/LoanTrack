using LoanTrack.Application.Common.CQRS;
using LoanTrack.Application.Customers.Queries;
using LoanTrack.Application.Finance;
using LoanTrack.Application.Loans;
using LoanTrack.Domain.Common;

namespace LoanTrack.Application.Dashboard;

public class GetDashboardQueryHandler(
    ICustomerQueryRepository customerRepo,
    ILoanQueryRepository loanRepo,
    IInstallmentQueryRepository installmentRepo,
    IFinanceJournalQueryRepository journalRepo
) : IQueryHandler<GetDashboardQuery, DashboardResponse>
{
    public async Task<Result<DashboardResponse>> Handle(GetDashboardQuery request, CancellationToken cancellationToken)
    {
        var customersCounts = await customerRepo.GetCustomersCountsByCenter(cancellationToken);
        var loanCountByStatus = await loanRepo.GetLoanCountsByStatusAsync(cancellationToken);
        var installmentsCount = await installmentRepo.GetInstallmentsCountForTodayAsync(cancellationToken);
        var overDueInstallmentsCount = await installmentRepo.GetOverDueInstallmentsCountAsync(cancellationToken);
       
        var today = DateOnly.FromDateTime(DateTime.Today);
        var startOfMonth = new DateOnly(today.Year, today.Month, 1);
        var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);
        var financeSummary = await journalRepo.GetFinanceSummaryAsync(startOfMonth, endOfMonth, cancellationToken);
        var collections = await installmentRepo.GetCollectionAsync(startOfMonth, endOfMonth, cancellationToken);

        return new DashboardResponse
        {
            CustomersCountsByCentre = customersCounts,
            LoanCountsByStatus = loanCountByStatus,
            InstallmentCounts = (DueToday: installmentsCount, Overdue: overDueInstallmentsCount),
            Collections = collections.Value,
            FinanceSummary = financeSummary.Value
        };
    }
}
