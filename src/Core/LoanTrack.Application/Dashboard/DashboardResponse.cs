using LoanTrack.Application.Finance.Reports.GetFinanceSummary;
using LoanTrack.Application.Loans.Queries.Reports.GetCollection;

namespace LoanTrack.Application.Dashboard;

public record DashboardResponse
{
    public IReadOnlyList<(string Center, string Group, int Count)> CustomersCountsByCentre { get; init; }
    public IReadOnlyList<(string Status, int Count)> LoanCountsByStatus { get; init; }
    public (int DueToday, int Overdue) InstallmentCounts { get; init; }
    public CollectionResponse Collections { get; init; }
    public FinanceSummaryResponse FinanceSummary { get; init; }
}
